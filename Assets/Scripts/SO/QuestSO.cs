using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;
using HarmonyLib;
using UnityEditor;
using UnityEngine.Events;

public enum QuestType
{
    Daily,
    Weekly,
    Achievement
}

public enum GoalType
{
    Killing
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Ennisia/Quest")]
public class QuestSO : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public int ID;
        public string Name;
        public QuestType QuestType;
        public string Description;
    }

    [Header("Info")] public Info Information;

    [System.Serializable]
    public struct Stat
    {
        public int Energy;
        public SerializedDictionary<Currency, int> CurrencyList;
    }

    [Header("Reward")] public Stat Reward = new()
        { Energy = 20, CurrencyList = new SerializedDictionary<Currency, int>() };

    public bool Completed { get; protected set; }
    public QuestCompletedEvent QuestCompleted;

    public abstract class QuestGoal : ScriptableObject
    {
        protected string Description;
        public int CurrentAmount { get; protected set; }
        public int RequiredAmount = 1;
        public bool Completed { get; protected set; }
        [HideInInspector] public UnityEvent GoalCompleted;

        public virtual string GetDescription()
        {
            return Description;
        }

        public virtual void Initialize()
        {
            Completed = false;
        }

        protected void Evaluate()
        {
            if (CurrentAmount >= RequiredAmount) Complete();
        }

        public void Complete()
        {
            Completed = true;
            GoalCompleted.Invoke();
            GoalCompleted.RemoveAllListeners();
        }

        public void Skip()
        {
            //Pay to skip ?
        }
    }

    public List<QuestGoal> Goals = new();

    public void Initialize()
    {
        Completed = false;
        QuestCompleted = new QuestCompletedEvent();
        foreach (var goal in Goals)
        {
            goal.Initialize();
            goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }

    private void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        if (!Completed) return;
        GiveRewards();
        QuestCompleted.Invoke(this);
        QuestCompleted.RemoveAllListeners();
    }
    private void GiveRewards()
    {
        foreach (var currency in Reward.CurrencyList)
        {
            PlayFabManager.Instance.AddCurrency(currency.Key, currency.Value);
        }

        PlayFabManager.Instance.AddEnergy(Reward.Energy);
    }
}

public class QuestCompletedEvent : UnityEvent<QuestSO>
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(QuestSO))]
public class QuestEditor : Editor
{
    private SerializedProperty _questInfoProperty;
    private SerializedProperty _questStatProperty;

    private List<string> _quesGoalType;
    private SerializedProperty _questGoalsListProperty;

    private Dictionary<QuestSO.QuestGoal, Editor> _goalEditors = new();
    private List<QuestSO.QuestGoal> _goalsToRemove = new();


    private void OnEnable()
    {
        _questInfoProperty = serializedObject.FindProperty(nameof(QuestSO.Information));
        _questStatProperty = serializedObject.FindProperty(nameof(QuestSO.Reward));


        _questGoalsListProperty = serializedObject.FindProperty(nameof(QuestSO.Goals));


        var lookup = typeof(QuestSO.QuestGoal);
        _quesGoalType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup)).Select(type => type.Name).ToList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var child = _questInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest Info", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        child = _questStatProperty.Copy();
        depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest Reward", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        EditorGUILayout.Space();

        var choice = EditorGUILayout.Popup("Add new Quest Goal", -1, _quesGoalType.ToArray());

        if (choice != -1)
        {
            var newInstance = CreateInstance(_quesGoalType[choice]) as QuestSO.QuestGoal;
            if (newInstance != null)
            {
                newInstance.name = _quesGoalType[choice];
                AssetDatabase.AddObjectToAsset(newInstance, serializedObject.targetObject);
                _questGoalsListProperty.arraySize++;
                _questGoalsListProperty.GetArrayElementAtIndex(_questGoalsListProperty.arraySize - 1)
                    .objectReferenceValue = newInstance;
                _goalEditors.Add(newInstance, CreateEditor(newInstance));
            }
        }

        EditorGUILayout.Space();

        for (var i = 0; i < _questGoalsListProperty.arraySize; i++)
        {
            var goalProperty = _questGoalsListProperty.GetArrayElementAtIndex(i);
            var goal = goalProperty.objectReferenceValue as QuestSO.QuestGoal;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUI.indentLevel++;

            var goalObject = new SerializedObject(goal);
            goalObject.Update();

            Editor goalEditor;
            if (_goalEditors.ContainsKey(goal))
            {
                goalEditor = _goalEditors[goal];
            }
            else
            {
                goalEditor = CreateEditor(goal);
                _goalEditors.Add(goal, goalEditor);
            }

            goalEditor.OnInspectorGUI();

            goalObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Remove", GUILayout.Width(80))) _goalsToRemove.Add(goal);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        // Remove the goals marked for removal
        foreach (var goalToRemove in _goalsToRemove)
        {
            var indexToRemove = GetGoalIndex(goalToRemove);
            if (indexToRemove < 0) continue;
            _questGoalsListProperty.DeleteArrayElementAtIndex(indexToRemove);
            DestroyImmediate(goalToRemove, true);
            _goalEditors.Remove(goalToRemove);
        }

        _goalsToRemove.Clear();

        serializedObject.ApplyModifiedProperties();
    }

    private int GetGoalIndex(QuestSO.QuestGoal goal)
    {
        for (var i = 0; i < _questGoalsListProperty.arraySize; i++)
        {
            var goalProperty = _questGoalsListProperty.GetArrayElementAtIndex(i);
            if (goalProperty.objectReferenceValue == goal) return i;
        }

        return -1;
    }

    private void OnDisable()
    {
        // Clean up goal editors
        foreach (var goalEditor in _goalEditors.Values.Where(goalEditor => goalEditor != null))
            DestroyImmediate(goalEditor);

        _goalEditors.Clear();
    }
}
#endif