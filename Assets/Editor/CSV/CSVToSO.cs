using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVToSO : EditorWindow
{
    private static Dictionary<int, SkillSO> _skillSOMap;
    private static Dictionary<string, List<Item.AttributeStat>> _equipmentTypes;
    private static int _currentLine;
    private static int _lines;

    enum TypeCSV 
    {
        supports, skills, equipment, weapons, missions, chapters
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
        if (GUILayout.Button("Skills"))
        {
            CreateScriptableObjectsFromCSV(TypeCSV.skills, "Skills");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Equipment Stats"))
        {
            _equipmentTypes = new();
            CreateScriptableObjectsFromCSV(TypeCSV.equipment, "EquipmentStats");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Weapons"))
        {
            CreateScriptableObjectsFromCSV(TypeCSV.weapons, "Weapons");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Chapters"))
        {
            CreateScriptableObjectsFromCSV(TypeCSV.chapters, "Chapters");
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Missions"))
        {
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
                MissionManager.MissionType missionType = GetMissionTypeFromFilePath(filePath);
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
                        LoadSkillSOs();
                        CreateSupportSO(rowData);
                        break;
                    case TypeCSV.skills:
                        CreateSkillDataSO(rowData);
                        break;
                    case TypeCSV.equipment:
                        CreateEquipmentStatDataSO(rowData);
                        break;
                    case TypeCSV.weapons:
                        LoadSkillSOs();
                        CreateWeaponSO(rowData);
                        break;
                    case TypeCSV.chapters:
                        CreateChapterSO(rowData);
                        break;
                    default:
                        break;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private MissionManager.MissionType GetMissionTypeFromFilePath(string filePath)
    {
        // Extract the mission type from the file name (e.g., "Mission-Explore" => "Explore")
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        string[] parts = fileNameWithoutExtension.Split('-');
        if (parts.Length >= 2)
        {
            if (Enum.TryParse(parts[1], out MissionManager.MissionType missionType))
            {
                return missionType;
            }
        }
        return MissionManager.MissionType.MainStory;
    }
    private static void LoadSkillSOs()
    {
        _skillSOMap = new Dictionary<int, SkillSO>();

        SkillSO[] skillSOs = Resources.LoadAll<SkillSO>("SO/Skills");

        foreach (SkillSO skillSO in skillSOs)
        {
            _skillSOMap[skillSO.Id] = skillSO;
        }
    }
    private static void AssignSkillData(Dictionary<string, string> rowData, string skillKey, ref SkillSO skillData)
    {
        int skillId;
        if (int.TryParse(rowData[skillKey], out skillId) && skillId != 0)
    {
        if (_skillSOMap.TryGetValue(skillId, out SkillSO skill))
        {
            skillData = skill;
        }
        else
{
    Debug.LogError($"Skill with ID {skillId} not found.");
}
    }
}
    private static void CreateSupportSO(Dictionary<string, string> rowData)
    {
        SupportCharacterSO scriptableObject = CreateInstance<SupportCharacterSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.Rarity = rowData["Rarity"];
        scriptableObject.Race = rowData["Race"];
        scriptableObject.Job = rowData["Class"];
        scriptableObject.Element = rowData["Element"];
        AssignSkillData(rowData, "PrimarySkill", ref scriptableObject.PrimarySkillData);
        AssignSkillData(rowData, "SecondarySkill", ref scriptableObject.SecondarySkillData);

        scriptableObject.Description = rowData["Description"].Replace("\"", string.Empty);
        scriptableObject.Catchphrase = rowData["CatchPhrase"].Replace("\"", string.Empty);

        string savePath = $"Assets/Resources/SO/SupportsCharacter/{scriptableObject.Rarity}/{scriptableObject.Id}-{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }
    private static void CreateSkillDataSO(Dictionary<string, string> rowData)
    {
        SkillSO scriptableObject = CreateInstance<SkillSO>();
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
        scriptableObject.IsPassive = bool.Parse(rowData["isPassive"]);

        string fileName = CSVUtils.GetFileName(scriptableObject.Name);
        string savePath = $"Assets/Resources/SO/Skills/{fileName}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);

        // Check if the .cs file already exists
        string scriptFilePath = $"Assets/Scripts/Skills/List/{fileName}.cs";
        if (!File.Exists(scriptFilePath))
        {
            // Create the .cs file
            using (StreamWriter writer = File.CreateText(scriptFilePath))
            {
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("");
                writer.WriteLine($"public class {fileName} : Skill");
                writer.WriteLine("{");
                writer.WriteLine($"//TODO -> {scriptableObject.Description}");
                writer.WriteLine("    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }");
                writer.WriteLine("\r\n    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }");
                writer.WriteLine("\r\n    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }");
                writer.WriteLine("\r\n    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }");
                writer.WriteLine("\r\n    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }");
                writer.WriteLine("\r\n    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }");
                writer.WriteLine("\r\n    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }");
                writer.WriteLine("\r\n    public override void TakeOffStats(List<Entity> targets, Entity player, int turn) { }");
                writer.WriteLine("}");
            }

            // Refresh the AssetDatabase to detect the newly created .cs file
            AssetDatabase.Refresh();
        }
        
    }
    private static void CreateEquipmentStatDataSO(Dictionary<string, string> rowData)
    {
        string[] rarities = Enum.GetNames(typeof(Item.ItemRarity));

        if (!_equipmentTypes.ContainsKey(rowData["type"]))
        {
            _equipmentTypes[rowData["type"]] = new List<Item.AttributeStat> { Enum.Parse<Item.AttributeStat>(CSVUtils.GetFileName(rowData["attribute"])) };
        }
        else
        {
            _equipmentTypes[rowData["type"]].Add(Enum.Parse<Item.AttributeStat>(CSVUtils.GetFileName(rowData["attribute"])));
        }

        if (_currentLine == _lines)
        {
            foreach (KeyValuePair<string, List<Item.AttributeStat>> type in _equipmentTypes)
            {
                EquipmentAttributesSO attributeSO = CreateInstance<EquipmentAttributesSO>();
                attributeSO.Attributes = type.Value;

                string savePath = $"Assets/Resources/SO/EquipmentStats/Attributes/{type.Key}.asset";
                AssetDatabase.CreateAsset(attributeSO, savePath);
            }
        }

        for (int i = 0; i < rarities.Length; i++)
        {
            StatMinMaxValuesSO valueSO = CreateInstance<StatMinMaxValuesSO>();
            valueSO.MinValue = int.Parse(rowData[$"{rarities[i].ToLower()}Min"]);
            valueSO.MaxValue = int.Parse(rowData[$"{rarities[i].ToLower()}Max"]);

            string savePath = $"Assets/Resources/SO/EquipmentStats/Values/{rarities[i]}_{rowData["attribute"]}.asset";
            AssetDatabase.CreateAsset(valueSO, savePath);
        }
    }
    private static void CreateWeaponSO(Dictionary<string, string> rowData)
    {
        GearSO scriptableObject = CreateInstance<GearSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"].Replace("\"", string.Empty);

        if (Enum.TryParse(rowData["Type"], out Item.GearType type))
        {
            scriptableObject.Type = type;
        }
        else
        {
            Debug.LogError($"Error parsing gear type for weapon {scriptableObject.Name}");
            return;
        }

        if (Enum.TryParse(rowData["Attribute"], out Item.AttributeStat attribute))
        {
            scriptableObject.Attribute = attribute;
        }
        else
        {
            Debug.LogError($"Error parsing gear attribute for weapon {scriptableObject.Name}");
            return;
        }

        scriptableObject.IsMagic = bool.Parse(rowData["isMagic"]);
        scriptableObject.StatValue = int.Parse(rowData["Value"]);

        AssignSkillData(rowData, "Skill1", ref scriptableObject.FirstSkillData);
        AssignSkillData(rowData, "Skill2", ref scriptableObject.SecondSkillData);

        scriptableObject.Description = rowData["Description"];

        string savePath = $"Assets/Resources/SO/Weapons/{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }
    private static void CreateMissionSO(Dictionary<string, string> rowData, MissionManager.MissionType type)
    {
        MissionSO scriptableObject = CreateInstance<MissionSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.EnergyCost = int.Parse(rowData["EnergyCost"]);
        scriptableObject.Unlocked = rowData["Unlocked"] == "VRAI";

        Dictionary<int, string> waves = new();
        HashSet<string> enemies = new HashSet<string>(); // Use HashSet to avoid duplicates
        int waveCount = 1;
        for (int i = 1; i <= 3; i++)
        {
            string wave = rowData[$"Wave{i}"];
            if (!wave.Equals("none"))
            {
                waves.Add(waveCount, wave);
                waveCount++;

                string[] waveEnemies = wave.Split(',');
                foreach (string enemy in waveEnemies)
                {
                    enemies.Add(enemy);
                }
            }
        }
        scriptableObject.Waves = waves;
        scriptableObject.WavesCount = waveCount;
        scriptableObject.Enemies = enemies.ToList();

        scriptableObject.DialogueId = int.Parse(rowData["IDDialogue"]);
        scriptableObject.ChapterId = int.Parse(rowData["IDChapter"]);
        scriptableObject.NumInChapter = int.Parse(rowData["Num"]);

        scriptableObject.Type = type;
        // Remove special characters and spaces from the mission name
        string missionName = CSVUtils.GetFileName(rowData["Name"]);

        string savePath = $"Assets/Resources/SO/Missions/{scriptableObject.Type}/{scriptableObject.ChapterId}.{scriptableObject.NumInChapter}-{missionName}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }
    private static void CreateChapterSO(Dictionary<string, string> rowData)
    {
        ChapterSO scriptableObject = CreateInstance<ChapterSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.ActId = int.Parse(rowData["ActID"]);
        scriptableObject.Name = rowData["Name"].Replace("\"", string.Empty);
        scriptableObject.NumberOfMission = int.Parse(rowData["NumberOfMission"]);

        string savePath = $"Assets/Resources/SO/Chapters/Act {scriptableObject.ActId}/Chapter-{scriptableObject.Id}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }
}