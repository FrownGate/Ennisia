using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine.Events;

public enum QuestType
{
    Daily,
    Weekly,
    Achievement
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
        public SerializedDictionary<Currency, int> currencyList;
    }

    [Header("Reward")] public Stat Reward = new()
        { Energy = 20, currencyList = new SerializedDictionary<Currency, int>() };

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

    public List<QuestGoal> Goals;

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
        if (Completed)
        {
            //give Rewards
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
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

    [MenuItem("Assets/Quest", priority = 0)]
    public static void CreateQuest()
    {
        var newQuest = CreateInstance<QuestSO>();

        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
    }

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

        int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, _quesGoalType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(_quesGoalType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance,target);

            _questGoalsListProperty.InsertArrayElementAtIndex(_questGoalsListProperty.arraySize);
            _questGoalsListProperty.GetArrayElementAtIndex((_questGoalsListProperty.arraySize - 1)).objectReferenceValue = newInstance;
        }

        Editor ed = null;

        int toDelete = -1;
        for (int i = 0; i < _questGoalsListProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = _questGoalsListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue,null,ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width((32))))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (toDelete != -1)
        {
            var item = _questGoalsListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            //need to do it twice, first null the entry, second remove it
            _questGoalsListProperty.DeleteArrayElementAtIndex(toDelete);
            _questGoalsListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif