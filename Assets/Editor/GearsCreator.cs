using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GearsCreator : EditorWindow
{
    private int _selectedTab = 0;  // Index of the selected tab
    private GUIContent[] _tabLabels = new GUIContent[]  // Labels for the tabs
    {
        new GUIContent("Full Set"),
        new GUIContent("One Gear")
    };

    private List<EquipmentData> _equipmentList = new List<EquipmentData>();  // List to store equipment data
    private Dictionary<EquipmentData, SerializedObject> _serializedObjects = new Dictionary<EquipmentData, SerializedObject>();  // Dictionary to store serialized objects for equipment data
    private Dictionary<string, GUIContent[]> _rarityOptions = new Dictionary<string, GUIContent[]>();  // Dictionary to store rarity options for each equipment type
    private Dictionary<string, string[]> _rarityPropertyNames = new Dictionary<string, string[]>();  // Dictionary to store rarity property names for each equipment type
    private Dictionary<EquipmentData, string> _selectedRarity = new Dictionary<EquipmentData, string>();  // Dictionary to store the selected rarity for each equipment data
    private Dictionary<string, string> _selectedAttribute = new Dictionary<string, string>();  // Dictionary to store the selected attribute for each equipment type
    private Dictionary<string, List<string>> _availableAttributes = new Dictionary<string, List<string>>();  // Dictionary to store available attributes for each equipment type
    private Vector2 _scrollPosition = Vector2.zero;  // Scroll position for the GUI
    private HashSet<string> _displayedTypes = new HashSet<string>();  // Set to track displayed equipment types

    private bool _csvRead = false;  // Flag to indicate if CSV data has been read

    [MenuItem("Tools/Gears Creator")]
    public static void ShowWindow()
    {
        GetWindow<GearsCreator>("Gears Creator");  // Method to display the Gears Creator window
    }

    private void SetupRarityOptions()
    {
        _rarityOptions.Clear();  // Clear the rarity options dictionary
        _rarityPropertyNames.Clear();  // Clear the rarity property names dictionary

        foreach (EquipmentData equipment in _equipmentList)
        {
            string[] properties = new string[] { "Common", "Rare", "Epic", "Legendary" };  // Rarity properties
            GUIContent[] options = new GUIContent[properties.Length];  // GUI content for rarity options

            for (int i = 0; i < properties.Length; i++)
            {
                options[i] = new GUIContent(properties[i]);  // Create GUI content for each rarity option
            }

            _rarityOptions[equipment.type] = options;  // Add rarity options to the dictionary for the equipment type
            _rarityPropertyNames[equipment.type] = properties;  // Add rarity property names to the dictionary for the equipment type
        }
    }

    private void OnGUI()
    {
        _selectedTab = GUILayout.Toolbar(_selectedTab, _tabLabels);  // Create a toolbar with the selected tab index and tab labels

        switch (_selectedTab)
        {
            case 0:
                // Content of tab 1
                FullSetGearGUI();  // Call method to display GUI for full set gear
                break;

            case 1:
                // Content of tab 2
                GearGUI();  // Call method to display GUI for one gear
                break;
        }
    }
    private void GearGUI()
    {
        if (!_csvRead)
        {
            ReadCSV();  // If CSV data has not been read, call the ReadCSV() method
            _csvRead = true;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Equipment List:");  // Display label for equipment list
        if (GUILayout.Button("Update"))
        {
            ReadCSV();  // Button to update the equipment list by calling the ReadCSV() method
        }
        EditorGUILayout.EndHorizontal();

        // Dropdown menu for selecting the equipment type
        int selectedTypeIndex = 0;
        string[] equipmentTypes = _equipmentList.Select(e => e.type).ToArray();  // Get the list of equipment types
        if (_displayedTypes.Count > 0)
        {
            string currentType = _displayedTypes.First();  // Get the first displayed equipment type
            selectedTypeIndex = Array.IndexOf(equipmentTypes, currentType);  // Get the index of the current type in the equipmentTypes array
        }
        int newSelectedTypeIndex = EditorGUILayout.Popup("Equipment Type:", selectedTypeIndex, equipmentTypes);  // Display a dropdown menu for selecting the equipment type
        string newSelectedType = equipmentTypes[newSelectedTypeIndex];  // Get the selected equipment type from the dropdown

        // Clear displayed types and selected attributes if the type selection changes
        if (newSelectedType != _displayedTypes.First())
        {
            _displayedTypes.Clear();  // Clear the displayed types set
            _selectedAttribute.Clear();  // Clear the selected attributes dictionary
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);  // Begin a scroll view

        // Filter equipment list based on selected type
        List<EquipmentData> filteredEquipment = _equipmentList.Where(e => e.type == newSelectedType).ToList();  // Filter the equipment list based on the selected type
        EquipmentData selectedEquipment = _equipmentList[0];  // Default selected equipment (first item in the equipment list)
        foreach (EquipmentData equipment in filteredEquipment)
        {
            if (_displayedTypes.Contains(equipment.type) && _selectedAttribute[equipment.type] != equipment.attribute)
            {
                continue;  // If the equipment type is already displayed and the selected attribute is different, skip this equipment
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);  // Begin a vertical group with a help box style

            EditorGUILayout.LabelField("Equipment: " + equipment.type);  // Display the equipment type label
            UpdateEquipmentPropertiesGUI(equipment);  // Call a method to update and display the equipment properties GUI

            EditorGUILayout.EndVertical();  // End the vertical group

            _displayedTypes.Add(equipment.type);  // Add the equipment type to the displayed types set
            selectedEquipment = equipment;  // Set the selected equipment to the current equipment
        }

        EditorGUILayout.EndScrollView();  // End the scroll view

        if (GUILayout.Button("Create Equipment Assets"))
        {
            CreateGearAssets(selectedEquipment);  // Button to create equipment assets by calling the CreateGearAssets() method with the selected equipment
        }
    }

    private void FullSetGearGUI()
    {
        if (!_csvRead)
        {
            ReadCSV();  // If CSV data has not been read, call the ReadCSV() method
            _csvRead = true;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Equipment List:");  // Display label for equipment list
        if (GUILayout.Button("Update"))
        {
            ReadCSV();  // Button to update the equipment list by calling the ReadCSV() method
        }
        EditorGUILayout.EndHorizontal();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);  // Begin a scroll view

        foreach (EquipmentData equipment in _equipmentList)
        {
            if (_displayedTypes.Contains(equipment.type) && _selectedAttribute[equipment.type] != equipment.attribute)
            {
                continue;  // If the equipment type is already displayed and the selected attribute is different, skip this equipment
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);  // Begin a vertical group with a help box style

            EditorGUILayout.LabelField("Equipment: " + equipment.type);  // Display the equipment type label
            UpdateEquipmentPropertiesGUI(equipment);  // Call a method to update and display the equipment properties GUI

            EditorGUILayout.EndVertical();  // End the vertical group

            _displayedTypes.Add(equipment.type);  // Add the equipment type to the displayed types set
        }

        EditorGUILayout.EndScrollView();  // End the scroll view

        if (GUILayout.Button("Create Equipment Assets"))
        {
            CreateFullSetGearAssets();  // Button to create full set gear assets by calling the CreateFullSetGearAssets() method
        }
    }

    private void UpdateEquipmentPropertiesGUI(EquipmentData equipment)
    {
        _serializedObjects[equipment].Update();  // Update the serialized object for the equipment

        string name = _serializedObjects[equipment].FindProperty("equipmentName").stringValue;  // Get the name property from the serialized object
        EditorGUI.BeginChangeCheck();
        name = EditorGUILayout.TextField("Name", name);  // Display a text field for editing the name
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(equipment, "Change name Value");  // Record an undo action for changing the name
            _serializedObjects[equipment].FindProperty("equipmentName").stringValue = name;  // Update the name property in the serialized object
            _serializedObjects[equipment].ApplyModifiedProperties();  // Apply modified properties to the serialized object
        }

        // Dropdown menu for selecting the rarity
        EditorGUILayout.BeginHorizontal();
        string[] properties = _rarityPropertyNames[equipment.type];  // Get the rarity property names based on the equipment type
        GUIContent[] options = _rarityOptions[equipment.type];  // Get the rarity options based on the equipment type

        int selectedRarityIndex = GetSelectedRarityIndex(equipment);  // Get the index of the selected rarity
        EditorGUILayout.LabelField("Rarity: ");
        int newSelectedRarityIndex = EditorGUILayout.Popup(selectedRarityIndex, options);  // Display a dropdown menu for selecting the rarity
        string newSelectedRarity = properties[newSelectedRarityIndex];  // Get the selected rarity from the dropdown

        if (newSelectedRarity != _selectedRarity[equipment])
        {
            Undo.RecordObject(equipment, "Change Rarity");  // Record an undo action for changing the rarity
            UpdateMinMaxValues(equipment, newSelectedRarity);  // Update the min/max values based on the selected rarity
            _selectedRarity[equipment] = newSelectedRarity;  // Update the selected rarity in the dictionary
        }
        EditorGUILayout.EndHorizontal();

        // Dropdown menu for selecting the attribute
        List<string> availableAttributes = _availableAttributes[equipment.type];  // Get the available attributes based on the equipment type
        int selectedAttributeIndex = availableAttributes.IndexOf(equipment.attribute);  // Get the index of the selected attribute
        int newSelectedAttributeIndex = EditorGUILayout.Popup("Attribute:", selectedAttributeIndex, availableAttributes.ToArray());  // Display a dropdown menu for selecting the attribute
        string newSelectedAttribute = availableAttributes[newSelectedAttributeIndex];  // Get the selected attribute from the dropdown

        float min, max;
        _selectedAttribute[equipment.type] = equipment.attribute;  // Update the selected attribute in the dictionary

        if (newSelectedAttribute != equipment.attribute)
        {
            Undo.RecordObject(equipment, "Change Attribute");  // Record an undo action for changing the attribute
            _selectedAttribute[equipment.type] = newSelectedAttribute;  // Update the selected attribute in the dictionary
            UpdateMinMaxValues(equipment, newSelectedRarity);  // Update the min/max values based on the selected rarity
        }

        min = _serializedObjects[equipment].FindProperty(_selectedRarity[equipment].ToLower() + "Min").floatValue;  // Get the minimum value based on the selected rarity
        max = _serializedObjects[equipment].FindProperty(_selectedRarity[equipment].ToLower() + "Max").floatValue;  // Get the maximum value based on the selected rarity
        float value = _serializedObjects[equipment].FindProperty("value").floatValue;  // Get the current value property

        // Update the corresponding min/max property
        EditorGUI.BeginChangeCheck();
        float newValue = EditorGUILayout.Slider(newSelectedRarity + " Value", value, min, max);  // Display a slider for editing the value within the min/max range
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(equipment, "Change Slider Value");  // Record an undo action for changing the value
            _serializedObjects[equipment].FindProperty("value").floatValue = newValue;  // Update the value property in the serialized object
            _serializedObjects[equipment].ApplyModifiedProperties();  // Apply modified properties to the serialized object
        }

        string description = _serializedObjects[equipment].FindProperty("description").stringValue;  // Get the description property from the serialized object
        EditorGUI.BeginChangeCheck();
        description = EditorGUILayout.TextField("Description", description);  // Display a text field for editing the description
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(equipment, "Change description Value");  // Record an undo action for changing the description
            _serializedObjects[equipment].FindProperty("description").stringValue = description;  // Update the description property in the serialized object
            _serializedObjects[equipment].ApplyModifiedProperties();  // Apply modified properties to the serialized object
        }
    }

    private void UpdateMinMaxValues(EquipmentData equipment, string newRarity)
    {
        SerializedObject serializedObject = _serializedObjects[equipment];  // Get the serialized object for the equipment
        SerializedProperty minProperty = serializedObject.FindProperty(newRarity.ToLower() + "Min");  // Find the serialized property for the new rarity's minimum value
        SerializedProperty maxProperty = serializedObject.FindProperty(newRarity.ToLower() + "Max");  // Find the serialized property for the new rarity's maximum value
        SerializedProperty valueProperty = serializedObject.FindProperty("value");  // Find the serialized property for the value

        // Get the current min/max values before changing the rarity
        float currentMin = minProperty.floatValue;
        float currentMax = maxProperty.floatValue;

        // Update the selected rarity to the new value
        valueProperty.floatValue = (currentMin + currentMax) / 2;  // Set the value to the average of the new min/max values

        // Update the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    private int GetSelectedRarityIndex(EquipmentData equipment)
    {
        string[] properties = _rarityPropertyNames[equipment.type];  // Get the rarity property names based on the equipment type
        string selectedRarity = _selectedRarity[equipment];  // Get the selected rarity for the equipment

        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i] == selectedRarity)
            {
                return i;  // Return the index of the selected rarity
            }
        }

        return 0;  // If the selected rarity is not found, return 0 as the default index
    }

    private void ReadCSV()
    {
        _equipmentList.Clear();
        _serializedObjects.Clear();
        _selectedRarity.Clear();
        _availableAttributes.Clear();

        string csvPath = "/Editor/CSV/EquipmentStats.csv";  // Path to the CSV file

        string[] lines = File.ReadAllLines(Application.dataPath + csvPath);  // Read all lines from the CSV file

        // Skip the header line
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');  // Split each line into values based on the comma delimiter

            EquipmentData equipment = CreateInstance<EquipmentData>();  // Create a new instance of EquipmentData

            // Set the properties of the equipment based on the CSV values
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

            _equipmentList.Add(equipment);  // Add the equipment to the list

            SerializedObject serializedObject = new SerializedObject(equipment);  // Create a serialized object for the equipment
            _serializedObjects[equipment] = serializedObject;  // Add the serialized object to the dictionary
            _selectedRarity[equipment] = "Common";  // Set the selected rarity for the equipment as "Common"

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

        SetupRarityOptions();  // Setup the rarity options and property names
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

            EquipmentSO equipmentSO = CreateEquipment(equipment);  // Create an EquipmentSO instance based on the EquipmentData

            // Determine the path for saving the asset based on the equipment name and type
            if (string.IsNullOrEmpty(equipmentSO.Name))
            {
                path = "Assets/Ressources/SO/EquippedGears/" + equipment.type + ".asset";
            }
            else
            {
                path = "Assets/Ressources/SO/EquippedGears/" + equipment.type + "-" + equipment.equipmentName + ".asset";
            }

            AssetDatabase.CreateAsset(equipmentSO, path);  // Create the asset at the specified path
        }

        AssetDatabase.SaveAssets();  // Save all created assets
        AssetDatabase.Refresh();  // Refresh the Asset Database
        EditorSceneManager.MarkAllScenesDirty();  // Mark all scenes as dirty to ensure changes are saved
    }

    private void CreateGearAssets(EquipmentData equipment)
    {
        string path;

        EquipmentSO equipmentSO = CreateEquipment(equipment);  // Create an EquipmentSO instance based on the EquipmentData

        // Determine the path for saving the asset based on the equipment name and type
        if (string.IsNullOrEmpty(equipmentSO.Name))
        {
            path = "Assets/Ressources/SO/EquippedGears/" + equipment.type + ".asset";
        }
        else
        {
            path = "Assets/Ressources/SO/EquippedGears/" + equipment.equipmentName + ".asset";
        }

        AssetDatabase.CreateAsset(equipmentSO, path);  // Create the asset at the specified path

        AssetDatabase.SaveAssets();  // Save all created assets
        AssetDatabase.Refresh();  // Refresh the Asset Database
        EditorSceneManager.MarkAllScenesDirty();  // Mark all scenes as dirty to ensure changes are saved
    }

    private EquipmentSO CreateEquipment(EquipmentData equipmentData)
    {
        EquipmentSO equipment = CreateInstance<EquipmentSO>();  // Create a new instance of EquipmentSO

        // Set the properties of the EquipmentSO based on the EquipmentData
        equipment.Name = equipmentData.equipmentName;
        equipment.Type = equipmentData.type;
        equipment.Rarity = _selectedRarity[equipmentData];
        equipment.Attribute = equipmentData.attribute;
        equipment.Value = equipmentData.value;
        equipment.Description = equipmentData.description;

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
