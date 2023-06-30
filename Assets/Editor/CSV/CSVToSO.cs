using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using Assets.Plugins.SerializedCollections.Runtime.Scripts;

public class CSVToSO : EditorWindow
{
    private static Dictionary<int, SkillSO> _skillSOMap;
    private static Dictionary<string, List<Item.AttributeStat>> _equipmentTypes;
    private static int _currentLine;
    private static int _lines;

    private enum TypeCSV
    {
        supports,
        skills,
        equipment,
        weapons,
        missions,
        chapters,
        pets,
        quests
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
        if (GUILayout.Button("Supports")) CreateScriptableObjectsFromCSV(TypeCSV.supports, "Supports");
        GUILayout.Space(25);
        if (GUILayout.Button("Skills")) CreateScriptableObjectsFromCSV(TypeCSV.skills, "Skills");
        GUILayout.Space(25);
        if (GUILayout.Button("Equipment Stats"))
        {
            _equipmentTypes = new Dictionary<string, List<Item.AttributeStat>>();
            CreateScriptableObjectsFromCSV(TypeCSV.equipment, "EquipmentStats");
        }

        GUILayout.Space(25);
        if (GUILayout.Button("Weapons")) CreateScriptableObjectsFromCSV(TypeCSV.weapons, "Weapons");
        GUILayout.Space(25);
        if (GUILayout.Button("Chapters")) CreateScriptableObjectsFromCSV(TypeCSV.chapters, "Chapters");
        GUILayout.Space(25);
        if (GUILayout.Button("Missions")) CreateScriptableObjectsFromCSV(TypeCSV.missions, "Mission");
        GUILayout.Space(25);
        if (GUILayout.Button("Pets")) CreateScriptableObjectsFromCSV(TypeCSV.pets, "Pets");
        GUILayout.Space(25);
        if (GUILayout.Button("Quests")) CreateScriptableObjectsFromCSV(TypeCSV.quests, "Quests");
    }

    private void CreateScriptableObjectsFromCSV(TypeCSV type, string fileName)
    {
        if (type == TypeCSV.missions)
        {
            var directoryPath = Application.dataPath + "/Editor/CSV";
            var csvFiles = Directory.GetFiles(directoryPath, "Mission-*.csv");

            foreach (var filePath in csvFiles)
            {
                var lines = File.ReadAllLines(filePath);
                _lines = lines.Length;
                var missionType = GetMissionTypeFromFilePath(filePath);
                if (lines.Length <= 1)
                {
                    Debug.LogError($"CSV file '{Path.GetFileName(filePath)}' is empty or missing headers.");
                    continue;
                }

                var headers = lines[0].Split(',');

                for (var i = 1; i < lines.Length; i++)
                {
                    _currentLine = i + 1;
                    var values = CSVUtils.SplitCSVLine(lines[i]);

                    if (values.Length != headers.Length)
                    {
                        Debug.LogError(
                            $"Error parsing line {i + 1} in CSV file '{Path.GetFileName(filePath)}'. The number of values does not match the number of headers.");
                        continue;
                    }

                    Dictionary<string, string> rowData = new();

                    for (var j = 0; j < headers.Length; j++) rowData[headers[j]] = values[j];

                    CreateMissionSO(rowData, missionType);
                }
            }
        }
        else
        {
            var filePath = Application.dataPath + $"/Editor/CSV/{fileName}.csv";
            var lines = File.ReadAllLines(filePath);
            _lines = lines.Length;

            if (lines.Length <= 1)
            {
                Debug.LogError("CSV file is empty or missing headers.");
                return;
            }

            var headers = lines[0].Split(',');

            for (var i = 1; i < lines.Length; i++)
            {
                _currentLine = i + 1;
                var values = CSVUtils.SplitCSVLine(lines[i]);

                if (values.Length != headers.Length)
                {
                    Debug.LogError(
                        $"Error parsing line {i + 1} in CSV file. The number of values does not match the number of headers.");
                    continue;
                }

                Dictionary<string, string> rowData = new();

                for (var j = 0; j < headers.Length; j++) rowData[headers[j]] = values[j];

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
                    case TypeCSV.pets:
                        CreatePetSO(rowData);
                        break;
                    case TypeCSV.quests:
                        CreateQuestSO(rowData);
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
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        var parts = fileNameWithoutExtension.Split('-');
        if (parts.Length < 2) return MissionManager.MissionType.MainStory;
        return Enum.TryParse(parts[1], out MissionManager.MissionType missionType)
            ? missionType
            : MissionManager.MissionType.MainStory;
    }

    private static void LoadSkillSOs()
    {
        _skillSOMap = new Dictionary<int, SkillSO>();

        var skillSOs = Resources.LoadAll<SkillSO>("SO/Skills");

        foreach (var skillSO in skillSOs) _skillSOMap[skillSO.Id] = skillSO;
    }

    private static void AssignSkillData(Dictionary<string, string> rowData, string skillKey, ref SkillSO skillData)
    {
        if (!int.TryParse(rowData[skillKey], out var skillId) || skillId == 0) return;
        if (_skillSOMap.TryGetValue(skillId, out var skill))
            skillData = skill;
        else
            Debug.LogError($"Skill with ID {skillId} not found.");
    }

    #region CreateSO

    private static void CreateSupportSO(Dictionary<string, string> rowData)
    {
        var scriptableObject = CreateInstance<SupportCharacterSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.Rarity = rowData["Rarity"];
        scriptableObject.Race = rowData["Race"];
        scriptableObject.Job = rowData["Class"];
        scriptableObject.Element = Enum.Parse<Element.ElementType>(rowData["Element"]);
        AssignSkillData(rowData, "PrimarySkill", ref scriptableObject.PrimarySkillData);
        AssignSkillData(rowData, "SecondarySkill", ref scriptableObject.SecondarySkillData);

        scriptableObject.Description = rowData["Description"].Replace("\"", string.Empty);
        scriptableObject.Catchphrase = rowData["CatchPhrase"].Replace("\"", string.Empty);

        var savePath =
            $"Assets/Resources/SO/SupportsCharacter/{scriptableObject.Rarity}/{scriptableObject.Id}-{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateSkillDataSO(Dictionary<string, string> rowData)
    {
        var scriptableObject = CreateInstance<SkillSO>();
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

        var fileName = CSVUtils.GetFileName(scriptableObject.Name);
        var savePath = $"Assets/Resources/SO/Skills/{fileName}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);

        // Check if the .cs file already exists
        var scriptFilePath = $"Assets/Scripts/Skills/List/{fileName}.cs";
        if (File.Exists(scriptFilePath)) return;
        // Create the .cs file
        using (var writer = File.CreateText(scriptFilePath))
        {
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("");
            writer.WriteLine($"public class {fileName} : Skill");
            writer.WriteLine("{");
            writer.WriteLine($"//TODO -> {scriptableObject.Description}");
            writer.WriteLine(
                "    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }");
            writer.WriteLine(
                "\r\n    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }");
            writer.WriteLine(
                "\r\n    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }");
            writer.WriteLine(
                "\r\n    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }");
            writer.WriteLine(
                "\r\n    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }");
            writer.WriteLine(
                "\r\n    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }");
            writer.WriteLine(
                "\r\n    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }");
            writer.WriteLine(
                "\r\n    public override void TakeOffStats(List<Entity> targets, Entity player, int turn) { }");
            writer.WriteLine("}");
        }

        // Refresh the AssetDatabase to detect the newly created .cs file
        AssetDatabase.Refresh();
    }

    private static void CreateEquipmentStatDataSO(Dictionary<string, string> rowData)
    {
        var rarities = Enum.GetNames(typeof(Item.ItemRarity));

        if (!_equipmentTypes.ContainsKey(rowData["type"]))
            _equipmentTypes[rowData["type"]] = new List<Item.AttributeStat>
                { Enum.Parse<Item.AttributeStat>(rowData["attribute"]) };
        else
            _equipmentTypes[rowData["type"]].Add(Enum.Parse<Item.AttributeStat>(rowData["attribute"]));

        if (_currentLine == _lines)
            foreach (var type in _equipmentTypes)
            {
                var attributeSO = CreateInstance<EquipmentAttributesSO>();
                attributeSO.Attributes = type.Value;

                var savePath = $"Assets/Resources/SO/EquipmentStats/Attributes/{type.Key}.asset";
                AssetDatabase.CreateAsset(attributeSO, savePath);
            }

        foreach (var rarity in rarities)
        {
            StatMinMaxValuesSO valueSO = CreateInstance<StatMinMaxValuesSO>();
            valueSO.MinValue = int.Parse(rowData[$"{rarity.ToLower()}Min"]);
            valueSO.MaxValue = int.Parse(rowData[$"{rarity.ToLower()}Max"]);

            string savePath = $"Assets/Resources/SO/EquipmentStats/Values/{rarity}_{rowData["attribute"]}.asset";
            AssetDatabase.CreateAsset(valueSO, savePath);
        }
    }

    private static void CreateWeaponSO(Dictionary<string, string> rowData)
    {
        var scriptableObject = CreateInstance<GearSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"].Replace("\"", string.Empty);
        scriptableObject.Type = Item.GearType.Weapon;

        if (Enum.TryParse(rowData["Type"], out Item.GearWeaponType type))
        {
            scriptableObject.WeaponType = type;
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

        var savePath = $"Assets/Resources/SO/Weapons/{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateMissionSO(Dictionary<string, string> rowData, MissionManager.MissionType type)
    {
        var scriptableObject = CreateInstance<MissionSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"];
        scriptableObject.EnergyCost = int.Parse(rowData["EnergyCost"]);
        scriptableObject.Unlocked = bool.Parse(rowData["Unlocked"]);

        SerializedDictionary<int, List<string>> waves = new();
        HashSet<string> enemies = new(); // Use HashSet to avoid duplicates
        List<string> enemiesInWaveList = new();
        var waveCount = 0;
        var i = 1;
        while (rowData.ContainsKey($"Wave{i}"))
        {
            var wave = rowData[$"Wave{i}"];
            if (!wave.Equals(""))
            {
                var waveEnemies = wave.Split(',');
                foreach (var enemy in waveEnemies)
                {
                    if (enemy == "Talk Only") continue;
                    enemiesInWaveList.Add(enemy);
                    enemies.Add(enemy);
                }
                waves.Add(waveCount, enemiesInWaveList);
                waveCount++;
            }

            i++;
        }

        scriptableObject.Waves = waves;
        scriptableObject.WavesCount = waveCount;
        scriptableObject.Enemies = enemies.ToList();

        scriptableObject.DialogueId = int.Parse(rowData["IDDialogue"]);
        scriptableObject.ChapterId = int.Parse(rowData["IDChap"]);
        scriptableObject.NumInChapter = int.Parse(rowData["Num"]);

        scriptableObject.Type = type;

        var currencies = rowData.ToList();
        for (var ii = i; ii < rowData.Count; ii++)
        {
            var Rewardtype = currencies[ii].Key;
            if (!Enum.TryParse(Rewardtype, out PlayFabManager.GameCurrency currencyType)) continue;
            Debug.Log(rowData[Rewardtype]);

            scriptableObject.CurrencyRewards.Add(currencyType, int.Parse(rowData[Rewardtype]));
        }

        var gears = rowData["Gear"].Split(",");
        foreach (var gearReward in gears)
        {
            if (!Enum.TryParse(gearReward, out Item.ItemRarity itemRarity)) continue;

            scriptableObject.GearReward.Add(itemRarity);
        }

        scriptableObject.Experience = int.Parse(rowData["XP"]);

        // Remove special characters and spaces from the mission name
        var missionName = CSVUtils.GetFileName(rowData["Name"]);


        var savePath =
            $"Assets/Resources/SO/Missions/{scriptableObject.Type}/{scriptableObject.ChapterId}.{scriptableObject.NumInChapter}-{missionName}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateChapterSO(Dictionary<string, string> rowData)
    {
        var scriptableObject = CreateInstance<ChapterSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.ActId = int.Parse(rowData["ActID"]);
        scriptableObject.Name = rowData["Name"].Replace("\"", string.Empty);
        scriptableObject.NumberOfMission = int.Parse(rowData["NumberOfMission"]);

        var savePath = $"Assets/Resources/SO/Chapters/Act {scriptableObject.ActId}/Chapter-{scriptableObject.Id}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreatePetSO(Dictionary<string, string> rowData)
    {
        var scriptableObject = CreateInstance<PetSO>();
        scriptableObject.Id = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"].Replace("\"", string.Empty);
        scriptableObject.Lore = rowData["Lore"];

        var savePath = $"Assets/Resources/SO/Pets/{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    private static void CreateQuestSO(Dictionary<string, string> rowData)
    {
        var scriptableObject = CreateInstance<QuestSO>();
        scriptableObject.ID = int.Parse(rowData["ID"]);
        scriptableObject.Name = rowData["Name"].Replace("\"", string.Empty);
        scriptableObject.Description = rowData["Description"];
        scriptableObject.Energy = int.Parse(rowData["Energy"]);

        var currencies = rowData.ToList();
        for (var i = 4; i < rowData.Count; i++)
        {
            var type = currencies[i].Key;
            if (!Enum.TryParse(type, out PlayFabManager.GameCurrency currencyType)) continue;
            Debug.Log(type);

            scriptableObject.currencyList.Add(currencyType, int.Parse(rowData[type]));
        }

        var savePath = $"Assets/Resources/SO/Quests/{scriptableObject.ID}-{scriptableObject.Name}.asset";
        AssetDatabase.CreateAsset(scriptableObject, savePath);
    }

    #endregion
}