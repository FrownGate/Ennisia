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

    private List<GearData> _gearList = new();  // List to store equipment data
    private Dictionary<GearData, SerializedObject> _serializedObjects = new();  // Dictionary to store serialized objects for equipment data
    private Dictionary<string, GUIContent[]> _rarityOptions = new();  // Dictionary to store rarity options for each equipment type
    private Dictionary<string, Item.ItemRarity[]> _rarityPropertyNames = new();  // Dictionary to store rarity property names for each equipment type
    private Dictionary<GearData, Item.ItemRarity> _selectedRarity = new();  // Dictionary to store the selected rarity for each equipment data
    private Dictionary<string, string> _selectedAttribute = new();  // Dictionary to store the selected attribute for each equipment type
    private Dictionary<string, List<string>> _availableAttributes = new();  // Dictionary to store available attributes for each equipment type
    private Vector2 _scrollPosition = Vector2.zero;  // Scroll position for the GUI
    private HashSet<string> _displayedTypes = new();  // Set to track displayed equipment types

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

        foreach (GearData gear in _gearList)
        {
            Item.ItemRarity[] properties = new Item.ItemRarity[] { //TODO -> optimize
                Item.ItemRarity.Common, Item.ItemRarity.Rare, Item.ItemRarity.Epic, Item.ItemRarity.Legendary
            };  // Rarity properties
            GUIContent[] options = new GUIContent[properties.Length];  // GUI content for rarity options

            for (int i = 0; i < properties.Length; i++)
            {
                options[i] = new GUIContent(properties[i].ToString());  // Create GUI content for each rarity option
            }

            _rarityOptions[gear.type.ToString()] = options;  // Add rarity options to the dictionary for the equipment type
            _rarityPropertyNames[gear.type.ToString()] = properties;  // Add rarity property names to the dictionary for the equipment type
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
        GUILayout.Label("Gear List:");  // Display label for equipment list
        if (GUILayout.Button("Update"))
        {
            ReadCSV();  // Button to update the equipment list by calling the ReadCSV() method
        }
        EditorGUILayout.EndHorizontal();

        // Dropdown menu for selecting the equipment type
        int selectedTypeIndex = 0;
        string[] gearTypes = _gearList.Select(g => g.type.ToString()).ToArray();  // Get the list of equipment types
        if (_displayedTypes.Count > 0)
        {
            string currentType = _displayedTypes.First();  // Get the first displayed equipment type
            selectedTypeIndex = Array.IndexOf(gearTypes, currentType);  // Get the index of the current type in the equipmentTypes array
        }
        int newSelectedTypeIndex = EditorGUILayout.Popup("Gear Type:", selectedTypeIndex, gearTypes);  // Display a dropdown menu for selecting the equipment type
        string newSelectedType = gearTypes[newSelectedTypeIndex];  // Get the selected equipment type from the dropdown

        // Clear displayed types and selected attributes if the type selection changes
        if (newSelectedType != _displayedTypes.First())
        {
            _displayedTypes.Clear();  // Clear the displayed types set
            _selectedAttribute.Clear();  // Clear the selected attributes dictionary
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);  // Begin a scroll view

        // Filter equipment list based on selected type
        List<GearData> filteredGear = _gearList.Where(g => g.type.ToString() == newSelectedType).ToList();  // Filter the equipment list based on the selected type
        GearData selectedGear = _gearList[0];  // Default selected equipment (first item in the equipment list)
        foreach (GearData gear in filteredGear)
        {
            if (_displayedTypes.Contains(gear.type.ToString()) && _selectedAttribute[gear.type.ToString()] != gear.attribute.ToString())
            {
                continue;  // If the equipment type is already displayed and the selected attribute is different, skip this equipment
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);  // Begin a vertical group with a help box style

            EditorGUILayout.LabelField("Gear: " + gear.type);  // Display the equipment type label
            UpdateEquipmentPropertiesGUI(gear);  // Call a method to update and display the equipment properties GUI

            EditorGUILayout.EndVertical();  // End the vertical group

            _displayedTypes.Add(gear.type.ToString());  // Add the equipment type to the displayed types set
            selectedGear = gear;  // Set the selected equipment to the current equipment
        }

        EditorGUILayout.EndScrollView();  // End the scroll view

        if (GUILayout.Button("Create Gear Assets"))
        {
            CreateGearAssets(selectedGear);  // Button to create equipment assets by calling the CreateGearAssets() method with the selected equipment
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
        GUILayout.Label("Gear List:");  // Display label for equipment list
        if (GUILayout.Button("Update"))
        {
            ReadCSV();  // Button to update the equipment list by calling the ReadCSV() method
        }
        EditorGUILayout.EndHorizontal();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);  // Begin a scroll view

        foreach (GearData gear in _gearList)
        {
            if (_displayedTypes.Contains(gear.type.ToString()) && _selectedAttribute[gear.type.ToString()] != gear.attribute.ToString())
            {
                continue;  // If the equipment type is already displayed and the selected attribute is different, skip this equipment
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);  // Begin a vertical group with a help box style

            EditorGUILayout.LabelField("Gear: " + gear.type);  // Display the equipment type label
            UpdateEquipmentPropertiesGUI(gear);  // Call a method to update and display the equipment properties GUI

            EditorGUILayout.EndVertical();  // End the vertical group

            _displayedTypes.Add(gear.type.ToString());  // Add the equipment type to the displayed types set
        }

        EditorGUILayout.EndScrollView();  // End the scroll view

        if (GUILayout.Button("Create Gear Assets"))
        {
            CreateFullSetGearAssets();  // Button to create full set gear assets by calling the CreateFullSetGearAssets() method
        }
    }

    private void UpdateEquipmentPropertiesGUI(GearData gear)
    {
        _serializedObjects[gear].Update();  // Update the serialized object for the equipment

        string name = _serializedObjects[gear].FindProperty("equipmentName").stringValue;  // Get the name property from the serialized object
        EditorGUI.BeginChangeCheck();
        name = EditorGUILayout.TextField("Name", name);  // Display a text field for editing the name
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(gear, "Change name Value");  // Record an undo action for changing the name
            _serializedObjects[gear].FindProperty("equipmentName").stringValue = name;  // Update the name property in the serialized object
            _serializedObjects[gear].ApplyModifiedProperties();  // Apply modified properties to the serialized object
        }

        // Dropdown menu for selecting the rarity
        EditorGUILayout.BeginHorizontal();
        Item.ItemRarity[] properties = _rarityPropertyNames[gear.type.ToString()];  // Get the rarity property names based on the equipment type
        GUIContent[] options = _rarityOptions[gear.type.ToString()];  // Get the rarity options based on the equipment type

        int selectedRarityIndex = GetSelectedRarityIndex(gear);  // Get the index of the selected rarity
        EditorGUILayout.LabelField("Rarity: ");
        int newSelectedRarityIndex = EditorGUILayout.Popup(selectedRarityIndex, options);  // Display a dropdown menu for selecting the rarity
        Item.ItemRarity newSelectedRarity = properties[newSelectedRarityIndex];  // Get the selected rarity from the dropdown

        if (newSelectedRarity != _selectedRarity[gear])
        {
            Undo.RecordObject(gear, "Change Rarity");  // Record an undo action for changing the rarity
            UpdateMinMaxValues(gear, newSelectedRarity);  // Update the min/max values based on the selected rarity
            _selectedRarity[gear] = newSelectedRarity;  // Update the selected rarity in the dictionary
        }
        EditorGUILayout.EndHorizontal();

        // Dropdown menu for selecting the attribute
        List<string> availableAttributes = _availableAttributes[gear.type.ToString()];  // Get the available attributes based on the equipment type
        int selectedAttributeIndex = availableAttributes.IndexOf(gear.attribute.ToString());  // Get the index of the selected attribute
        int newSelectedAttributeIndex = EditorGUILayout.Popup("Attribute:", selectedAttributeIndex, availableAttributes.ToArray());  // Display a dropdown menu for selecting the attribute
        string newSelectedAttribute = availableAttributes[newSelectedAttributeIndex];  // Get the selected attribute from the dropdown

        float min, max;
        _selectedAttribute[gear.type.ToString()] = gear.attribute.ToString();  // Update the selected attribute in the dictionary

        if (newSelectedAttribute != gear.attribute.ToString())
        {
            Undo.RecordObject(gear, "Change Attribute");  // Record an undo action for changing the attribute
            _selectedAttribute[gear.type.ToString()] = newSelectedAttribute;  // Update the selected attribute in the dictionary
            UpdateMinMaxValues(gear, newSelectedRarity);  // Update the min/max values based on the selected rarity
        }

        min = _serializedObjects[gear].FindProperty(_selectedRarity[gear].ToString().ToLower() + "Min").floatValue;  // Get the minimum value based on the selected rarity
        max = _serializedObjects[gear].FindProperty(_selectedRarity[gear].ToString().ToLower() + "Max").floatValue;  // Get the maximum value based on the selected rarity
        float value = _serializedObjects[gear].FindProperty("value").floatValue;  // Get the current value property

        // Update the corresponding min/max property
        EditorGUI.BeginChangeCheck();
        float newValue = EditorGUILayout.Slider(newSelectedRarity + " Value", value, min, max);  // Display a slider for editing the value within the min/max range
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(gear, "Change Slider Value");  // Record an undo action for changing the value
            _serializedObjects[gear].FindProperty("value").floatValue = newValue;  // Update the value property in the serialized object
            _serializedObjects[gear].ApplyModifiedProperties();  // Apply modified properties to the serialized object
        }

        string description = _serializedObjects[gear].FindProperty("description").stringValue;  // Get the description property from the serialized object
        EditorGUI.BeginChangeCheck();
        description = EditorGUILayout.TextField("Description", description);  // Display a text field for editing the description
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(gear, "Change description Value");  // Record an undo action for changing the description
            _serializedObjects[gear].FindProperty("description").stringValue = description;  // Update the description property in the serialized object
            _serializedObjects[gear].ApplyModifiedProperties();  // Apply modified properties to the serialized object
        }
    }

    private void UpdateMinMaxValues(GearData gear, Item.ItemRarity newRarity)
    {
        SerializedObject serializedObject = _serializedObjects[gear];  // Get the serialized object for the equipment
        SerializedProperty minProperty = serializedObject.FindProperty(newRarity.ToString().ToLower() + "Min");  // Find the serialized property for the new rarity's minimum value
        SerializedProperty maxProperty = serializedObject.FindProperty(newRarity.ToString().ToLower() + "Max");  // Find the serialized property for the new rarity's maximum value
        SerializedProperty valueProperty = serializedObject.FindProperty("value");  // Find the serialized property for the value

        // Get the current min/max values before changing the rarity
        float currentMin = minProperty.floatValue;
        float currentMax = maxProperty.floatValue;

        // Update the selected rarity to the new value
        valueProperty.floatValue = (currentMin + currentMax) / 2;  // Set the value to the average of the new min/max values

        // Update the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    private int GetSelectedRarityIndex(GearData gear)
    {
        Item.ItemRarity[] properties = _rarityPropertyNames[gear.type.ToString()];  // Get the rarity property names based on the equipment type
        Item.ItemRarity selectedRarity = _selectedRarity[gear];  // Get the selected rarity for the equipment

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
        _gearList.Clear();
        _serializedObjects.Clear();
        _selectedRarity.Clear();
        _availableAttributes.Clear();

        string csvPath = "/Editor/CSV/EquipmentStats.csv";  // Path to the CSV file

        string[] lines = File.ReadAllLines(Application.dataPath + csvPath);  // Read all lines from the CSV file

        // Skip the header line
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');  // Split each line into values based on the comma delimiter

            GearData gear = CreateInstance<GearData>();  // Create a new instance of EquipmentData

            // Set the properties of the equipment based on the CSV values
            gear.type = Enum.Parse<Item.GearType>(CSVUtils.GetFileName(values[0]));
            gear.attribute = Enum.Parse<Item.AttributeStat>(CSVUtils.GetFileName(values[1]));
            gear.commonMin = float.Parse(values[2]);
            gear.commonMax = float.Parse(values[3]);
            gear.rareMin = float.Parse(values[4]);
            gear.rareMax = float.Parse(values[5]);
            gear.epicMin = float.Parse(values[6]);
            gear.epicMax = float.Parse(values[7]);
            gear.legendaryMin = float.Parse(values[8]);
            gear.legendaryMax = float.Parse(values[9]);
            gear.value = (gear.commonMin + gear.commonMax) / 2;

            _gearList.Add(gear);  // Add the equipment to the list

            SerializedObject serializedObject = new SerializedObject(gear);  // Create a serialized object for the equipment
            _serializedObjects[gear] = serializedObject;  // Add the serialized object to the dictionary
            _selectedRarity[gear] = Item.ItemRarity.Common;  // Set the selected rarity for the equipment as "Common"

            // Add the attribute to the list of available attributes for the equipment type
            if (!_availableAttributes.ContainsKey(gear.type.ToString()))
            {
                _availableAttributes[gear.type.ToString()] = new List<string>();
            }

            if (!_availableAttributes[gear.type.ToString()].Contains(gear.attribute.ToString()))
            {
                _availableAttributes[gear.type.ToString()].Add(gear.attribute.ToString());
            }
        }

        SetupRarityOptions();  // Setup the rarity options and property names
    }

    private void CreateFullSetGearAssets()
    {
        string path;
        foreach (GearData gear in _gearList)
        {
            if (_displayedTypes.Contains(gear.type.ToString()) && _selectedAttribute[gear.type.ToString()] != gear.attribute.ToString())
            {
                continue;
            }

            GearSO gearSO = CreateGear(gear);  // Create an EquipmentSO instance based on the EquipmentData

            // Determine the path for saving the asset based on the equipment name and type
            if (string.IsNullOrEmpty(gearSO.Name))
            {
                path = "Assets/Resources/SO/GearsCreator/" + gear.type + ".asset";
            }
            else
            {
                path = "Assets/Resources/SO/GearsCreator/" + gear.type + "-" + gear.equipmentName + ".asset";
            }

            AssetDatabase.CreateAsset(gearSO, path);  // Create the asset at the specified path
        }

        AssetDatabase.SaveAssets();  // Save all created assets
        AssetDatabase.Refresh();  // Refresh the Asset Database
        EditorSceneManager.MarkAllScenesDirty();  // Mark all scenes as dirty to ensure changes are saved
    }

    private void CreateGearAssets(GearData gear)
    {
        string path;

        GearSO gearSO = CreateGear(gear);  // Create an EquipmentSO instance based on the EquipmentData

        // Determine the path for saving the asset based on the equipment name and type
        if (string.IsNullOrEmpty(gearSO.Name))
        {
            path = "Assets/Resources/SO/GearsCreator/" + gear.type + ".asset";
        }
        else
        {
            path = "Assets/Resources/SO/GearsCreator/" + gear.equipmentName + ".asset";
        }

        AssetDatabase.CreateAsset(gearSO, path);  // Create the asset at the specified path

        AssetDatabase.SaveAssets();  // Save all created assets
        AssetDatabase.Refresh();  // Refresh the Asset Database
        EditorSceneManager.MarkAllScenesDirty();  // Mark all scenes as dirty to ensure changes are saved
    }

    private GearSO CreateGear(GearData gearData)
    {
        GearSO gear = CreateInstance<GearSO>();  // Create a new instance of EquipmentSO

        // Set the properties of the EquipmentSO based on the EquipmentData
        gear.Name = gearData.equipmentName;
        gear.Type = gearData.type;
        gear.Rarity = _selectedRarity[gearData];
        gear.Attribute = gearData.attribute;
        gear.StatValue = gearData.value;
        gear.Description = gearData.description;

        return gear;
    }

}

public class GearData : ScriptableObject
{
    public string equipmentName;
    public Item.GearType type;
    public Item.AttributeStat attribute;
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
