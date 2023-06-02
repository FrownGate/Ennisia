using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class CSVToSO : EditorWindow
{
    private static Dictionary<string, List<string>> _equipmentTypes;
    private static int _currentLine;
    private static int _lines;

    enum TypeCSV
    {
        supports, enemies, skills, equipment, missions
    }

    [MenuItem("Tools/CSV to SO")]
    public static void ShowWindow()
    {
        GetWindow<CSVToSO>("CSV to SO");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Scriptable Objects from CSV", EditorStyles.boldLabel);
        GUILayout.Space(25);
        if (GUILayout.Button("Supports"))
        {
            CreateScriptableObjectsFromCSV(TypeCSV.supports, "Supports");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Enemies"))
        {
            CreateScriptableObjectsFromCSV(TypeCSV.enemies, "Enemies");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Skills"))
        {
            CreateScriptableObjectsFromCSV(TypeCSV.skills, "Skills");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Equipment Stats"))
        {
            _equipmentTypes = new Dictionary<string, List<string>>();
            CreateScriptableObjectsFromCSV(TypeCSV.equipment, "EquipmentStats");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Mission"))
        {
            _equipmentTypes = new Dictionary<string, List<string>>();
            CreateScriptableObjectsFromCSV(TypeCSV.missions, "Mission");
        }
    }
    private void CreateScriptableObjectsFromCSV(TypeCSV type, string fileName)
    {
        if (type == TypeCSV.missions)
        {
            string directoryPath = Application.dataPath + "/Editor/CSV";
            string[] csvFiles = Directory.GetFiles(directoryPath, "Mission-*.csv");

            foreach (string filePath in csvFiles)
            {
                string[] lines = File.ReadAllLines(filePath);
                _lines = lines.Length;
                MissionType missionType = GetMissionTypeFromFilePath(filePath);
                if (lines.Length <= 1)
                {
                    Debug.LogError($"CSV file '{Path.GetFileName(filePath)}' is empty or missing headers.");
                    continue;
                }

                string[] headers = lines[0].Split(',');

                for (int i = 1; i < lines.Length; i++)
                {
                    _currentLine = i + 1;
                    string[] values = CSVUtils.SplitCSVLine(lines[i]);

                    if (values.Length != headers.Length)
                    {
                        Debug.LogError($"Error parsing line {i + 1} in CSV file '{Path.GetFileName(filePath)}'. The number of values does not match the number of headers.");
                        continue;
                    }

                    Dictionary<string, string> rowData = new();

                    for (int j = 0; j < headers.Length; j++)
                    {
                        rowData[headers[j]] = values[j];
                    }

                    CreateMissionSO(rowData, missionType);
                }
            }
        }
        else
        {
            string filePath = Application.dataPath + $"/Editor/CSV/{fileName}.csv";
            string[] lines = File.ReadAllLines(filePath);
            _lines = lines.Length;

            if (lines.Length <= 1)
            {
                Debug.LogError("CSV file is empty or missing headers.");
                return;
            }

            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                _currentLine = i + 1;
                string[] values = CSVUtils.SplitCSVLine(lines[i]);

                if (values.Length != headers.Length)
                {
                    Debug.LogError($"Error parsing line {i + 1} in CSV file. The number of values does not match the number of headers.");
                    continue;
                }

                Dictionary<string, string> rowData = new();

                for (int j = 0; j < headers.Length; j++)
                {
                    rowData[headers[j]] = values[j];
                }

                switch (type)
                {
                    case TypeCSV.supports:
                        CreateSupportSO(rowData);
                        break;
                    case TypeCSV.enemies:
                        CreateEnemySO(rowData);
                        break;
                    case TypeCSV.skills:
                        CreateSkillDataSO(rowData);
                        break;
                    case TypeCSV.equipment:
                        CreateEquipmentStatDataSO(rowData);
                        break;
                    default:
                        break;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private MissionType GetMissionTypeFromFilePath(string filePath)
    {
        // Extract the mission type from the file name (e.g., "Mission-Explore" => "Explore")
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        string[] parts = fileNameWithoutExtension.Split('-');
        if (parts.Length >= 2)
        {
            if (Enum.TryParse<MissionType>(parts[1], out MissionType missionType))
            {
                return missionType;
            }
        }
        return MissionType.MainStory;
    }

    private static void CreateSupportSO(Dictionary<string, string> rowData)
    {
        SupportsCharactersSO scriptableObject = ScriptableObject.CreateInstance<SupportsCharactersSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.Rarity = rowData["Rarity"];
        scriptableObject.Race = rowData["Race"];
        scriptableObject.Job = rowData["Class"];
        scriptableObject.Element = rowData["Element"];
        scriptableObject.Description = rowData["Description"].Replace("\"", string.Empty);
        scriptableObject.Catchphrase = rowData["CatchPhrase"].Replace("\"", string.Empty);

        string savePath = $"Assets/Resources/SO/SupportsCharacter/{scriptableObject.Rarity}/{scriptableObject.Id}-{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateEnemySO(Dictionary<string, string> rowData)
    {
        //EnemiesSO scriptableObject = ScriptableObject.CreateInstance<EnemiesSO>();
        //scriptableObject.id = int.Parse(rowData["ID"]);
        //scriptableObject.suppportName = rowData["Name"];
        //scriptableObject.rarity = rowData["Rarity"];
        //scriptableObject.race = rowData["Race"];
        //scriptableObject.supportClass = rowData["Class"];
        //scriptableObject.passif = int.Parse(rowData["Passif"]);
        //scriptableObject.skill = int.Parse(rowData["Skill"]);
        //scriptableObject.description = rowData["Description"].Replace("\"", string.Empty);
        //scriptableObject.catchPhrase = rowData["CatchPhrase"].Replace("\"", string.Empty);

        //string savePath = $"Assets/Resources/SO/Enemies/{scriptableObject.id}-{scriptableObject.suppportName}.asset";
        //AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateSkillDataSO(Dictionary<string, string> rowData)
    {
        SkillData scriptableObject = ScriptableObject.CreateInstance<SkillData>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["skillName"];
        scriptableObject.Description = rowData["description"].Replace("\"", string.Empty);
        scriptableObject.DamageAmount = float.Parse(rowData["damageAmount"]);
        scriptableObject.ShieldAmount = float.Parse(rowData["shieldAmount"]);
        scriptableObject.HealingAmount = float.Parse(rowData["healingAmount"]);
        scriptableObject.IgnoreDef = float.Parse(rowData["penDef"]);
        scriptableObject.HitNumber = int.Parse(rowData["hitNb"]);
        scriptableObject.MaxCooldown = int.Parse(rowData["maxCooldown"]);
        scriptableObject.IsAfter = bool.Parse(rowData["isAfter"]);
        scriptableObject.AOE = bool.Parse(rowData["AOE"]);
        scriptableObject.IsMagic = bool.Parse(rowData["isMagic"]);

        string savePath = $"Assets/Resources/SO/Skills/{scriptableObject.Name.Replace(" ", string.Empty).Replace("\u2019", string.Empty).Replace("!", string.Empty).Replace("'", string.Empty)}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateEquipmentStatDataSO(Dictionary<string, string> rowData)
    {
        string[] rarities = new string[4] { "Common", "Rare", "Epic", "Legendary" };

        if (!_equipmentTypes.ContainsKey(rowData["type"]))
        {
            _equipmentTypes[rowData["type"]] = new List<string> { rowData["attribute"].Replace(" ", string.Empty) };
        }
        else
        {
            _equipmentTypes[rowData["type"]].Add(rowData["attribute"].Replace(" ", string.Empty));
        }

        if (_currentLine == _lines)
        {
            foreach (KeyValuePair<string, List<string>> type in _equipmentTypes)
            {
                EquipmentAttributeSO attributeSO = CreateInstance<EquipmentAttributeSO>();
                attributeSO.Attributes = type.Value;

                string savePath = $"Assets/Resources/SO/EquipmentStats/Attributes/{type.Key}.asset";
                AssetDatabase.CreateAsset(attributeSO, savePath);
            }
        }

        for (int i = 0; i < rarities.Length; i++)
        {
            EquipmentValueSO valueSO = CreateInstance<EquipmentValueSO>();
            valueSO.MinValue = int.Parse(rowData[$"{rarities[i].ToLower()}Min"]);
            valueSO.MaxValue = int.Parse(rowData[$"{rarities[i].ToLower()}Max"]);

            string savePath = $"Assets/Resources/SO/EquipmentStats/Values/{rowData["type"]}_{rarities[i]}_{rowData["attribute"]}.asset";
            AssetDatabase.CreateAsset(valueSO, savePath);
        }
    }
    private static void CreateMissionSO(Dictionary<string, string> rowData,MissionType type)
    {
        MissionSO scriptableObject = ScriptableObject.CreateInstance<MissionSO>();
        scriptableObject.ID = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.EnergyCost = int.Parse(rowData["EnergyCost"]);
        scriptableObject.Unlocked = rowData["Unlocked"] == "VRAI";

        Dictionary<int, string> waves = new();
        int waveCount = 1;
        for (int i = 1; i <= 3; i++)
        {
            string wave = rowData[$"Wave{i}"];
            if (!wave.Equals("none"))
            {
                waves.Add(waveCount, wave);
                waveCount++;
            }
        }
        scriptableObject.Waves = waves;
        scriptableObject.WavesCount = waveCount;

        scriptableObject.DialogueId = int.Parse(rowData["IDDialogue"]);
        scriptableObject.ChapID = int.Parse(rowData["IDChap"]);

        scriptableObject.Type = type;
        // Remove special characters and spaces from the mission name
        string missionName = rowData["Name"];
        missionName = Regex.Replace(missionName, @"[^0-9a-zA-Z]+", ""); // Remove non-alphanumeric characters
        missionName = missionName.Replace(" ", ""); // Remove spaces

        string savePath = $"Assets/Resources/SO/Missions/{scriptableObject.Type}/{scriptableObject.ChapID}-{missionName}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

}
