using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GearsCreator : EditorWindow
{
    private int _selectedTab = 0;
    private GUIContent[] _tabLabels = new GUIContent[]
    {
        new GUIContent("Full Set"),
        new GUIContent("One Gear")
    };

    private List<EquipmentData> _equipmentList = new List<EquipmentData>();
    private Dictionary<EquipmentData, SerializedObject> _serializedObjects = new Dictionary<EquipmentData, SerializedObject>();
    private Dictionary<string, GUIContent[]> _rarityOptions = new Dictionary<string, GUIContent[]>();
    private Dictionary<string, string[]> _rarityPropertyNames = new Dictionary<string, string[]>();
    private Dictionary<EquipmentData, string> _selectedRarity = new Dictionary<EquipmentData, string>();
    private Dictionary<string, string> _selectedAttribute = new Dictionary<string, string>();
    private Dictionary<string, List<string>> _availableAttributes = new Dictionary<string, List<string>>();
    private Vector2 _scrollPosition = Vector2.zero;
    private HashSet<string> _displayedTypes = new HashSet<string>();

    private bool _csvRead = false;
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

            _rarityOptions[equipment.type] = options;
            _rarityPropertyNames[equipment.type] = properties;
        }
    }

    private void OnGUI()
    {
        _selectedTab = GUILayout.Toolbar(_selectedTab, _tabLabels);

        switch (_selectedTab)
        {
            case 0:
                // Contenu de l'onglet 1
                FullSetGearGUI();
                break;

            case 1:
                // Contenu de l'onglet 2
                GearGUI();
                break;
        }
    }

    private void GearGUI()
    {
        if (!_csvRead)
        {
            ReadCSV();
            _csvRead = true;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Equipment List:");
        if (GUILayout.Button("Update"))
        {
            ReadCSV();
        }
        EditorGUILayout.EndHorizontal();

        // Dropdown menu for selecting the equipment type
        int selectedTypeIndex = 0;
        string[] equipmentTypes = _equipmentList.Select(e => e.type).ToArray();
        if (_displayedTypes.Count > 0)
        {
            string currentType = _displayedTypes.First();
            selectedTypeIndex = Array.IndexOf(equipmentTypes, currentType);
        }
        int newSelectedTypeIndex = EditorGUILayout.Popup("Equipment Type:", selectedTypeIndex, equipmentTypes);
        string newSelectedType = equipmentTypes[newSelectedTypeIndex];

        // Clear displayed types and selected attributes if the type selection changes
        if (newSelectedType != _displayedTypes.First())
        {
            _displayedTypes.Clear();
            _selectedAttribute.Clear();
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        // Filter equipment list based on selected type
        List<EquipmentData> filteredEquipment = _equipmentList.Where(e => e.type == newSelectedType).ToList();
        EquipmentData selectedEquipment = _equipmentList[0];
        foreach (EquipmentData equipment in filteredEquipment)
        {
            if (_displayedTypes.Contains(equipment.type) && _selectedAttribute[equipment.type] != equipment.attribute)
            {
                continue;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Equipment: " + equipment.type);
            UpdateEquipmentPropertiesGUI(equipment);

            EditorGUILayout.EndVertical();

            _displayedTypes.Add(equipment.type);
            selectedEquipment = equipment;
        }

        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Create Equipment Assets"))
        {
            CreateGearAssets(selectedEquipment);
        }
    }


    private void FullSetGearGUI()
    {
        if (!_csvRead)
        {
            ReadCSV();
            _csvRead = true;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Equipment List:");
        if (GUILayout.Button("Update"))
        {
            ReadCSV();
        }
        EditorGUILayout.EndHorizontal();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);


        foreach (EquipmentData equipment in _equipmentList)
        {
            if (_displayedTypes.Contains(equipment.type) && _selectedAttribute[equipment.type] != equipment.attribute)
            {
                continue;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Equipment: " + equipment.type);
            UpdateEquipmentPropertiesGUI(equipment);

            EditorGUILayout.EndVertical();

            _displayedTypes.Add(equipment.type);
        }

        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Create Equipment Assets"))
        {
            CreateFullSetGearAssets();
        }
    }

    private void UpdateEquipmentPropertiesGUI(EquipmentData equipment)
    {
        _serializedObjects[equipment].Update();
        string name = _serializedObjects[equipment].FindProperty("equipmentName").stringValue;
        EditorGUI.BeginChangeCheck();
        name = EditorGUILayout.TextField("Name", name);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(equipment, "Change name Value");
            _serializedObjects[equipment].FindProperty("equipmentName").stringValue = name;
            _serializedObjects[equipment].ApplyModifiedProperties();
        }
        // Dropdown menu for selecting the rarity
        EditorGUILayout.BeginHorizontal();
        string[] properties = _rarityPropertyNames[equipment.type];
        GUIContent[] options = _rarityOptions[equipment.type];

        int selectedRarityIndex = GetSelectedRarityIndex(equipment);
        EditorGUILayout.LabelField("Rarity: ");
        int newSelectedRarityIndex = EditorGUILayout.Popup(selectedRarityIndex, options);
        string newSelectedRarity = properties[newSelectedRarityIndex];

        if (newSelectedRarity != _selectedRarity[equipment])
        {
            // The selected rarity has changed, update the min/max values accordingly
            Undo.RecordObject(equipment, "Change Rarity");
            UpdateMinMaxValues(equipment, newSelectedRarity);
            _selectedRarity[equipment] = newSelectedRarity;
        }
        EditorGUILayout.EndHorizontal();
        // Dropdown menu for selecting the attribute
        List<string> availableAttributes = _availableAttributes[equipment.type];
        int selectedAttributeIndex = availableAttributes.IndexOf(equipment.attribute);
        int newSelectedAttributeIndex = EditorGUILayout.Popup("Attribute:", selectedAttributeIndex, availableAttributes.ToArray());
        string newSelectedAttribute = availableAttributes[newSelectedAttributeIndex];
        float min, max;
        _selectedAttribute[equipment.type] = equipment.attribute;
        if (newSelectedAttribute != equipment.attribute)
        {
            Undo.RecordObject(equipment, "Change Attribute");
            _selectedAttribute[equipment.type] = newSelectedAttribute;
            UpdateMinMaxValues(equipment, newSelectedRarity);
        }


        min = _serializedObjects[equipment].FindProperty(_selectedRarity[equipment].ToLower() + "Min").floatValue;
        max = _serializedObjects[equipment].FindProperty(_selectedRarity[equipment].ToLower() + "Max").floatValue;
        float value = _serializedObjects[equipment].FindProperty("value").floatValue;

        // Update the corresponding min/max property
        EditorGUI.BeginChangeCheck();
        float newValue = EditorGUILayout.Slider(newSelectedRarity + " Value", value, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(equipment, "Change Slider Value");
            _serializedObjects[equipment].FindProperty("value").floatValue = newValue;
            _serializedObjects[equipment].ApplyModifiedProperties();
        }

        string description = _serializedObjects[equipment].FindProperty("description").stringValue;
        EditorGUI.BeginChangeCheck();
        name = EditorGUILayout.TextField("Description", description);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(equipment, "Change description Value");
            _serializedObjects[equipment].FindProperty("description").stringValue = description;
            _serializedObjects[equipment].ApplyModifiedProperties();
        }
    }

    private void UpdateMinMaxValues(EquipmentData equipment, string newRarity)
    {
        SerializedObject serializedObject = _serializedObjects[equipment];
        SerializedProperty minProperty = serializedObject.FindProperty(newRarity.ToLower() + "Min");
        SerializedProperty maxProperty = serializedObject.FindProperty(newRarity.ToLower() + "Max");
        SerializedProperty valueProperty = serializedObject.FindProperty("value");

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
        string[] properties = _rarityPropertyNames[equipment.type];
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
        _selectedRarity.Clear();
        _availableAttributes.Clear();

        //string csvPath = EditorUtility.OpenFilePanel("Select CSV file", "", "csv");
        string csvPath = "/Editor/CSV/EquipmentStats.csv";


        string[] lines = File.ReadAllLines(Application.dataPath + csvPath);

        // Skip the header line
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            EquipmentData equipment = CreateInstance<EquipmentData>();
            equipment.type = values[0];
            equipment.attribute = values[1];
            equipment.commonMin = float.Parse(values[2]);
            equipment.commonMax = float.Parse(values[3]);
            equipment.rareMin = float.Parse(values[4]);
            equipment.rareMax = float.Parse(values[5]);
            equipment.epicMin = float.Parse(values[6]);
            equipment.epicMax = float.Parse(values[7]);
            equipment.legendaryMin = float.Parse(values[8]);
            equipment.legendaryMax = float.Parse(values[9]);
            equipment.value = (equipment.commonMin + equipment.commonMax) / 2;

            _equipmentList.Add(equipment);

            SerializedObject serializedObject = new SerializedObject(equipment);
            _serializedObjects[equipment] = serializedObject;
            _selectedRarity[equipment] = "Common";


            // Add the attribute to the list of available attributes for the equipment type
            if (!_availableAttributes.ContainsKey(equipment.type))
            {
                _availableAttributes[equipment.type] = new List<string>();
            }

            if (!_availableAttributes[equipment.type].Contains(equipment.attribute))
            {
                _availableAttributes[equipment.type].Add(equipment.attribute);
            }
        }

        SetupRarityOptions();
    }
    private void CreateFullSetGearAssets()
    {
        string path;
        foreach (EquipmentData equipment in _equipmentList)
        {
            if (_displayedTypes.Contains(equipment.type) && _selectedAttribute[equipment.type] != equipment.attribute)
            {
                continue;
            }
            EquipmentSO equipmentSO = CreateEquipment(equipment);
            if (equipmentSO.equipmentName == null || equipmentSO.equipmentName == "")
            {
                path = "Assets/Equipments/DebugGears/" + equipment.type + ".asset";

            }
            else
            {
                path = "Assets/Equipments/DebugGears/" + equipment.equipmentName + ".asset";
            }
            AssetDatabase.CreateAsset(equipmentSO, path);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorSceneManager.MarkAllScenesDirty();
    }

    private void CreateGearAssets(EquipmentData equipment)
    {
        string path;


        EquipmentSO equipmentSO = CreateEquipment(equipment);
        if (equipmentSO.equipmentName == null || equipmentSO.equipmentName == "")
        {
            path = "Assets/Equipments/DebugGears/" + equipment.type + ".asset";

        }
        else
        {
            path = "Assets/Equipments/DebugGears/" + equipment.equipmentName + ".asset";
        }
        AssetDatabase.CreateAsset(equipmentSO, path);


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorSceneManager.MarkAllScenesDirty();
    }

    private EquipmentSO CreateEquipment(EquipmentData equipmentData)
    {
        EquipmentSO equipment = CreateInstance<EquipmentSO>();
        equipment.equipmentName = equipmentData.equipmentName;
        equipment.type = equipmentData.type;
        equipment.rarity = _selectedRarity[equipmentData];
        equipment.attribute = equipmentData.attribute;
        equipment.value = equipmentData.value;
        equipment.description = equipmentData.description;

        return equipment;
    }
}

public class EquipmentData : ScriptableObject
{
    public string equipmentName;
    public string type;
    public string attribute;
    public float commonMin;
    public float commonMax;
    public float rareMin;
    public float rareMax;
    public float epicMin;
    public float epicMax;
    public float legendaryMin;
    public float legendaryMax;
    public float value;
    public string description;
}
