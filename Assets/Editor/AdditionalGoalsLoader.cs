#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using static QuestSO;
using System.Linq;

public static class AdditionalGoalsLoader
{
    public static void LoadAdditionalGoals(QuestSO questSO, string questSOPath)
    {
        // Load the additional goals from the CSV file
        var additionalGoalsFilePath = Application.dataPath + "/Editor/CSV/QuestsGoals.csv";

        if (!File.Exists(additionalGoalsFilePath))
        {
            Debug.LogError($"Additional goals CSV file not found at path: {additionalGoalsFilePath}");
            return;
        }

        var additionalGoalsLines = File.ReadAllLines(additionalGoalsFilePath);

        // Check if the QuestSO asset is dirty
        var isDirty = false;

        // Iterate through the additional goals CSV file
        for (var i = 1; i < additionalGoalsLines.Length; i++)
        {
            var additionalGoalData = CSVUtils.SplitCSVLine(additionalGoalsLines[i]);

            if (additionalGoalData.Length < 4)
            {
                Debug.LogError($"Error parsing additional goal data on line {i + 1} in the Goals.csv file.");
                continue;
            }

            // Extract the quest ID and goal data
            var questIdString = additionalGoalData[0];
            if (!int.TryParse(questIdString, out var questId))
            {
                Debug.LogError($"Invalid quest ID '{questIdString}' on line {i + 1} in the Goals.csv file.");
                continue;
            }

            var goalTypeString = additionalGoalData[1];
            if (!Enum.TryParse(goalTypeString, out GoalType goalType))
            {
                Debug.LogError($"Invalid goal type '{goalTypeString}' on line {i + 1} in the Goals.csv file.");
                continue;
            }

            var goalValue = additionalGoalData[2];
            var goalAmount = additionalGoalData[3];
            var goalSame = additionalGoalData[4];
            // Check if the additional goal is associated with the current quest
            if (questId != questSO.Information.ID)
                continue;

            // Create a new instance of the goal type
            var goalInstance = CreateGoal(goalType, questSO);
            if (goalInstance == null)
            {
                Debug.LogError($"Invalid goal type '{goalType}' on line {i + 1} in the Goals.csv file.");
                continue;
            }

            // Set the goal properties
            switch (goalType)
            {
                case GoalType.Killing:
                    if (!KillingProperties(goalInstance, goalValue))
                    {
                        Debug.LogError(
                            $"Error setting killing properties for goal on line {i + 1} in the Goals.csv file.");
                        continue;
                    }

                    break;
                case GoalType.Mission:
                    if (!MissionProperties(goalInstance, goalValue))
                    {
                        Debug.LogError(
                            $"Error setting mission properties for goal on line {i + 1} in the Goals.csv file.");
                        continue;
                    }

                    break;

                case GoalType.LevelUp:
                    if (!LevelUpProperties(goalInstance, goalValue))
                    {
                        Debug.LogError(
                            $"Error setting level up properties for goal on line {i + 1} in the Goals.csv file.");
                        continue;
                    }

                    break;
                case GoalType.GearMaxLevel:
                    if (!GearMaxLevelProperties(goalInstance, goalValue))
                    {
                        Debug.LogError(
                            $"Error setting level up properties for goal on line {i + 1} in the Goals.csv file.");
                        continue;
                    }

                    break;
                default:
                    Debug.LogError($"Unknown goal type: {goalType}");
                    continue;
            }

            if (!int.TryParse(goalAmount, out var amount))
            {
                Debug.LogError($"Invalid goal amount '{goalAmount}' on line {i + 1} in the Goals.csv file.");
                continue;
            }

            goalInstance.RequiredAmount = amount;

            if (!bool.TryParse(goalSame, out var same))
            {
                Debug.LogError($"Invalid goal amount '{goalSame}' on line {i + 1} in the Goals.csv file.");
                continue;
            }

            goalInstance.Same = same;

            // Add the goal to the QuestSO's Goals list
            questSO.Goals.Add(goalInstance);

            // Mark the QuestSO asset as dirty
            isDirty = true;
        }

        // Save changes to the QuestSO asset if it's dirty
        if (isDirty)
        {
            EditorUtility.SetDirty(questSO);
            AssetDatabase.SaveAssets();
        }

        // Import the sub-assets to the main asset
        AssetDatabase.ImportAsset(questSOPath, ImportAssetOptions.ForceUpdate);
    }


    private static QuestGoal CreateGoal(GoalType goalType, QuestSO questSO)
    {
        switch (goalType)
        {
            case GoalType.Killing:
                var killingGoal = ScriptableObject.CreateInstance<KillingGoal>();
                killingGoal.name = goalType.ToString();
                AssetDatabase.AddObjectToAsset(killingGoal, questSO);
                return killingGoal;
            // Add more cases for different goal types if needed
            case GoalType.Mission:
                var missionGoal = ScriptableObject.CreateInstance<MissionGoal>();
                missionGoal.name = goalType.ToString();
                AssetDatabase.AddObjectToAsset(missionGoal, questSO);

                return missionGoal;
            case GoalType.LevelUp:
                var levelUpGoal = ScriptableObject.CreateInstance<LevelUpGoal>();
                levelUpGoal.name = goalType.ToString();
                AssetDatabase.AddObjectToAsset(levelUpGoal, questSO);

                return levelUpGoal;
            case GoalType.GearMaxLevel:
                var gearLevelMax = ScriptableObject.CreateInstance<GearMaxLevelGoal>();
                gearLevelMax.name = goalType.ToString();
                AssetDatabase.AddObjectToAsset(gearLevelMax, questSO);
                return gearLevelMax;
            default:
                Debug.LogError($"Unknown goal type: {goalType}");
                return null;
        }
    }

    private static bool KillingProperties(QuestGoal goal, string goalValue)
    {
        var enemiesSO = Resources.LoadAll<EnemySO>("SO/Enemies");

        var values = CSVUtils.SplitCSVLine(goalValue);

        var isValid = true;

        foreach (var value in values)
            switch (value)
            {
                case "all":
                    var allEnemies = enemiesSO.ToList();
                    if (goal is KillingGoal killingGoal)
                    {
                        killingGoal.ToKill.AddRange(allEnemies);
                    }
                    else
                    {
                        Debug.LogError("Invalid goal type for KillingGoal.");
                        isValid = false;
                    }

                    break;
                case "allboss":
                    var allBossEnemies = enemiesSO.Where(enemy => enemy.IsBoss).ToList();
                    if (goal is KillingGoal killingGoalAllBoss)
                    {
                        killingGoalAllBoss.ToKill.AddRange(allBossEnemies);
                    }
                    else
                    {
                        Debug.LogError("Invalid goal type for KillingGoal (AllBoss).");
                        isValid = false;
                    }

                    break;
                case "allnonboss":
                    var allNonBossEnemies = enemiesSO.Where(enemy => !enemy.IsBoss).ToList();
                    if (goal is KillingGoal killingGoalAllNonBoss)
                    {
                        killingGoalAllNonBoss.ToKill.AddRange(allNonBossEnemies);
                    }
                    else
                    {
                        Debug.LogError("Invalid goal type for KillingGoal (AllNonBoss).");
                        isValid = false;
                    }

                    break;
                default:
                    var enemy = enemiesSO.FirstOrDefault(e => e.Name == value);
                    if (enemy != null)
                    {
                        if (goal is KillingGoal killingGoalSomeByName)
                        {
                            killingGoalSomeByName.ToKill.Add(enemy);
                        }
                        else
                        {
                            Debug.LogError("Invalid goal type for KillingGoal (SomeByName).");
                            isValid = false;
                        }
                    }
                    else
                    {
                        Debug.LogError($"Enemy '{value}' not found for goal.");
                        isValid = false;
                    }

                    break;
            }

        return isValid;
    }

    private static bool MissionProperties(QuestGoal goal, string goalValue)
    {
        var missionTypes = Enum.GetValues(typeof(MissionType));
        List<MissionSO> missionSos = new();

        foreach (MissionType type in missionTypes)
            missionSos.AddRange(Resources.LoadAll<MissionSO>($"SO/Missions/{type}").ToList());


        var values = CSVUtils.SplitCSVLine(goalValue);

        var isValid = true;

        foreach (var value in values)
            if (value == "all")
            {
                if (goal is MissionGoal missionGoal)
                {
                    missionGoal.ToDo.AddRange(missionSos);
                }
                else
                {
                    Debug.LogError("Invalid goal type for MissionGoal.");
                    isValid = false;
                }
            }
            else if (int.TryParse(value, out var missionId))
            {
                var missionById = missionSos.FirstOrDefault(mission => mission.Id == missionId);
                if (missionById != null)
                {
                    if (goal is MissionGoal missionGoal)
                    {
                        missionGoal.ToDo.Add(missionById);
                    }
                    else
                    {
                        Debug.LogError("Invalid goal type for MissionGoal.");
                        isValid = false;
                    }
                }
                else
                {
                    Debug.LogError($"Mission with ID {missionId} not found.");
                    isValid = false;
                }
            }
            else if (Enum.TryParse(value, out MissionType missionType))
            {
                var missionsOfType = missionSos.Where(mission => mission.Type == missionType).ToList();
                if (goal is MissionGoal missionGoal)
                {
                    missionGoal.ToDo.AddRange(missionsOfType);
                }
                else
                {
                    Debug.LogError("Invalid goal type for MissionGoal.");
                    isValid = false;
                }
            }
            else
            {
                Debug.LogError($"Invalid mission value: {value}.");
                isValid = false;
            }

        return isValid;
    }

    private static bool LevelUpProperties(QuestGoal goal, string goalValue)
    {

        var isValid = true;
        var values = CSVUtils.SplitCSVLine(goalValue);
        foreach (var value in values)
            if (Enum.TryParse(value, out LevelUpQuestEvent.LvlType type))
            {
                if (goal is LevelUpGoal levelUpGoal) levelUpGoal.lvlType = type;
            }
            else
            {
                Debug.LogError($"Invalid levelup value: {value}.");
                isValid = false;
            }

        return isValid;
    }

    private static bool GearMaxLevelProperties(QuestGoal goal, string goalValue)
    {
        var isValid = true;
        var values = CSVUtils.SplitCSVLine(goalValue);
        foreach (var value in values)
            switch (value)
            {
                case "gear":
                {
                    foreach (GearType type in Enum.GetValues(typeof(GearType)))
                    {
                        if (goal is not GearMaxLevelGoal gearMaxLevelGoal) continue;
                        if (type == GearType.Weapon) continue;
                        gearMaxLevelGoal.Type.Add(type);
                    }

                    break;
                }
                case "weapon":
                {
                    if (goal is not GearMaxLevelGoal gearMaxLevelGoal) continue;
                    gearMaxLevelGoal.Type.Add(GearType.Weapon);
                    break;
                }
                default:
                    Debug.LogError($"Invalid levelup value: {value}.");
                    isValid = false;
                    break;
            }

        return isValid;
    }
}
#endif