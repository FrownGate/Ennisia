using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GearsCreator : EditorWindow
{
    private List<EquipmentData> _equipmentList = new List<EquipmentData>();
    private Dictionary<EquipmentData, SerializedObject> _serializedObjects = new Dictionary<EquipmentData, SerializedObject>();
    private Dictionary<EquipmentData, Dictionary<string, float>> _originalValues = new Dictionary<EquipmentData, Dictionary<string, float>>();
    private Dictionary<string, GUIContent[]> _rarityOptions = new Dictionary<string, GUIContent[]>();
    private Dictionary<string, string[]> _rarityPropertyNames = new Dictionary<string, string[]>();
    private Dictionary<EquipmentData, string> _selectedRarity = new Dictionary<EquipmentData, string>();
    private Dictionary<string, List<string>> _availableAttributes = new Dictionary<string, List<string>>();
    private Vector2 _scrollPosition = Vector2.zero;

    [MenuItem("Tools/Gears Creator")]
    public static void ShowWindow()
    {
        GetWindow<GearsCreator>("Gears Creator");
    }

    private void SetupRarityOptions()
    {
        _rarityOptions.Clear();
        _rarityPropertyNames.Clear();

        foreach (EquipmentData equipment in _equipmentList)
        {
            string[] properties = new string[] { "Common", "Rare", "Epic", "Legendary" };
            GUIContent[] options = new GUIContent[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                options[i] = new GUIContent(properties[i]);
            }

            _rarityOptions[equipment.Type] = options;
            _rarityPropertyNames[equipment.Type] = properties;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Read CSV"))
        {
            ReadCSV();
        }

        GUILayout.Label("Equipment List:");

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        HashSet<string> displayedTypes = new HashSet<string>();

        foreach (EquipmentData equipment in _equipmentList)
        {
            if (displayedTypes.Contains(equipment.Type))
            {
                continue;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Equipment: " + equipment.Type);

            _serializedObjects[equipment].Update();

            // Dropdown menu for selecting the rarity
            string[] properties = _rarityPropertyNames[equipment.Type];
            GUIContent[] options = _rarityOptions[equipment.Type];

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

            // Dropdown menu for selecting the attribute
            List<string> availableAttributes = _availableAttributes[equipment.Type];
            int selectedAttributeIndex = availableAttributes.IndexOf(equipment.Attribute);
            int newSelectedAttributeIndex = EditorGUILayout.Popup("Attribute:", selectedAttributeIndex, availableAttributes.ToArray());
            string newSelectedAttribute = availableAttributes[newSelectedAttributeIndex];

            if (newSelectedAttribute != equipment.Attribute)
            {
                Undo.RecordObject(equipment, "Change Attribute");
                equipment.Attribute = newSelectedAttribute;
                UpdateMinMaxValues(equipment, newSelectedRarity);
            }

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

            displayedTypes.Add(equipment.Type);
        }

        EditorGUILayout.EndScrollView();
    }

    private void UpdateMinMaxValues(EquipmentData equipment, string newRarity)
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

    private int GetSelectedRarityIndex(EquipmentData equipment)
    {
        string[] properties = _rarityPropertyNames[equipment.Type];
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

    private void ReadCSV()
    {
        _equipmentList.Clear();
        _serializedObjects.Clear();
        _originalValues.Clear();
        _selectedRarity.Clear();
        _availableAttributes.Clear();

        string csvPath = EditorUtility.OpenFilePanel("Select CSV file", "", "csv");

        if (string.IsNullOrEmpty(csvPath))
        {
            return;
        }

        string[] lines = System.IO.File.ReadAllLines(csvPath);

        // Skip the header line
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            EquipmentData equipment = new EquipmentData();
            equipment.Type = values[0];
            equipment.Attribute = values[1];
            equipment.CommonMin = float.Parse(values[2]);
            equipment.CommonMax = float.Parse(values[3]);
            equipment.RareMin = float.Parse(values[4]);
            equipment.RareMax = float.Parse(values[5]);
            equipment.EpicMin = float.Parse(values[6]);
            equipment.EpicMax = float.Parse(values[7]);
            equipment.LegendaryMin = float.Parse(values[8]);
            equipment.LegendaryMax = float.Parse(values[9]);
            equipment.Value = (equipment.CommonMin + equipment.CommonMax) / 2;

            _equipmentList.Add(equipment);

            SerializedObject serializedObject = new SerializedObject(equipment);
            _serializedObjects[equipment] = serializedObject;
            _selectedRarity[equipment] = "Common";

            Dictionary<string, float> valuesDict = new Dictionary<string, float>
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

            _originalValues[equipment] = valuesDict;

            // Add the attribute to the list of available attributes for the equipment type
            if (!_availableAttributes.ContainsKey(equipment.Type))
            {
                _availableAttributes[equipment.Type] = new List<string>();
            }

            if (!_availableAttributes[equipment.Type].Contains(equipment.Attribute))
            {
                _availableAttributes[equipment.Type].Add(equipment.Attribute);
            }
        }

        SetupRarityOptions();
    }
}

public class EquipmentData : ScriptableObject
{
    public string Type;
    public string Attribute;
    public float CommonMin;
    public float CommonMax;
    public float RareMin;
    public float RareMax;
    public float EpicMin;
    public float EpicMax;
    public float LegendaryMin;
    public float LegendaryMax;
    public float Value;
}
