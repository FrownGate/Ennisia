using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GearsCreator : EditorWindow
{
    private List<EquipmentSO> _equipmentList = new List<EquipmentSO>();
    private Dictionary<EquipmentSO, SerializedObject> _serializedObjects = new Dictionary<EquipmentSO, SerializedObject>();
    private Dictionary<EquipmentSO, Dictionary<string, float>> _originalValues = new Dictionary<EquipmentSO, Dictionary<string, float>>();

    private Dictionary<string, GUIContent[]> _rarityOptions = new Dictionary<string, GUIContent[]>();
    private Dictionary<string, string[]> _rarityPropertyNames = new Dictionary<string, string[]>();
    private Dictionary<EquipmentSO, string> _selectedRarity = new Dictionary<EquipmentSO, string>();


    private Vector2 _scrollPosition = Vector2.zero;

    [MenuItem("Tools/Gears Creator")]
    public static void ShowWindow()
    {
        GetWindow<GearsCreator>("Gears Creator");
    }

    private void ReadScriptableObjects()
    {
        _equipmentList.Clear();
        _serializedObjects.Clear();
        _originalValues.Clear();
        _selectedRarity.Clear();

        string folderPath = "Assets/Equipments/DebugGears";
        string[] assetGuids = AssetDatabase.FindAssets("t:EquipmentSO", new[] { folderPath });

        foreach (string guid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            EquipmentSO equipment = AssetDatabase.LoadAssetAtPath<EquipmentSO>(assetPath);

            _equipmentList.Add(equipment);
            SerializedObject serializedObject = new SerializedObject(equipment);
            _serializedObjects[equipment] = serializedObject;
            _selectedRarity[equipment] = "Common";

            equipment.Value = (equipment.CommonMin + equipment.CommonMax) / 2;
            Dictionary<string, float> values = new Dictionary<string, float>
            {
                { "CommonMin", equipment.CommonMin },
                { "CommonMax", equipment.CommonMax },
                { "RareMin", equipment.RareMin },
                { "RareMax", equipment.RareMax },
                { "EpicMin", equipment.EpicMin },
                { "EpicMax", equipment.EpicMax },
                { "LegendaryMin", equipment.LegendaryMin },
                { "LegendaryMax", equipment.LegendaryMax },
                { "Value", equipment.Value }
            };

            _originalValues[equipment] = values;
            _selectedRarity[equipment] = "Common";
        }

        SetupRarityOptions();
    }

    private void SetupRarityOptions()
    {
        _rarityOptions.Clear();
        _rarityPropertyNames.Clear();

        foreach (EquipmentSO equipment in _equipmentList)
        {
            string[] properties = new string[] { "Common", "Rare", "Epic", "Legendary" };
            GUIContent[] options = new GUIContent[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                options[i] = new GUIContent(properties[i]);
            }

            _rarityOptions[equipment.name] = options;
            _rarityPropertyNames[equipment.name] = properties;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Read Scriptable Objects"))
        {
            ReadScriptableObjects();
        }

        GUILayout.Label("Equipment List:");

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        foreach (EquipmentSO equipment in _equipmentList)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Equipment: " + equipment.Name);

            _serializedObjects[equipment].Update();

            string[] properties = _rarityPropertyNames[equipment.name];
            GUIContent[] options = _rarityOptions[equipment.name];

            int selectedRarityIndex = GetSelectedRarityIndex(equipment);
            int newSelectedRarityIndex = EditorGUILayout.Popup(selectedRarityIndex, options);
            string newSelectedRarity = properties[newSelectedRarityIndex];

            if (newSelectedRarity != _selectedRarity[equipment])
            {
                // The selected rarity has changed, update the min/max values accordingly
                Undo.RecordObject(equipment, "Change Rarity");
                UpdateMinMaxValues(equipment, newSelectedRarity);
                _selectedRarity[equipment] = newSelectedRarity;
            }


            EditorGUILayout.LabelField("Type:", equipment.Type);

            float min = _serializedObjects[equipment].FindProperty(_selectedRarity[equipment] + "Min").floatValue;
            float max = _serializedObjects[equipment].FindProperty(_selectedRarity[equipment] + "Max").floatValue;
            float value = _serializedObjects[equipment].FindProperty("Value").floatValue;

            // Update the corresponding min/max property
            EditorGUI.BeginChangeCheck();
            float newValue = EditorGUILayout.Slider(newSelectedRarity + " Value", value, _originalValues[equipment][newSelectedRarity + "Min"], _originalValues[equipment][newSelectedRarity + "Max"]);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(equipment, "Change Slider Value");
                _serializedObjects[equipment].FindProperty("Value").floatValue = newValue;
                _serializedObjects[equipment].ApplyModifiedProperties();
            }


            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    private void UpdateMinMaxValues(EquipmentSO equipment, string newRarity)
    {
        SerializedObject serializedObject = _serializedObjects[equipment];
        SerializedProperty minProperty = serializedObject.FindProperty(newRarity + "Min");
        SerializedProperty maxProperty = serializedObject.FindProperty(newRarity + "Max");
        SerializedProperty valueProperty = serializedObject.FindProperty("Value");

        // Get the current min/max values before changing the rarity
        float currentMin = minProperty.floatValue;
        float currentMax = maxProperty.floatValue;

        // Update the selected rarity to the new value
        valueProperty.floatValue = (currentMin + currentMax) / 2;
        // Update the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    private int GetSelectedRarityIndex(EquipmentSO equipment)
    {
        string[] properties = _rarityPropertyNames[equipment.name];
        string selectedRarity = _selectedRarity[equipment];

        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i] == selectedRarity)
            {
                return i;
            }
        }

        return 0;
    }

    //A voir Plus  Tard
    //private void OnEnable()
    //{
    //    Undo.undoRedoPerformed += HandleUndoRedo;
    //}

    //private void OnDisable()
    //{
    //    Undo.undoRedoPerformed -= HandleUndoRedo;
    //}

    //private void HandleUndoRedo()
    //{
    //    foreach (EquipmentSO equipment in _equipmentList)
    //    {
    //        SerializedObject serializedObject = new SerializedObject(equipment);
    //        _serializedObjects[equipment] = serializedObject;

    //        Dictionary<string, float> values = _originalValues[equipment];



    //    }

    //}
}
