#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using static QuestSO;

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
        {
            var found = false;

            foreach (var enemySo in enemiesSO)
            {
                if (enemySo.Name != value) continue;
                var killingGoal = goal as KillingGoal;
                if (killingGoal != null)
                {
                    killingGoal.ToKill.Add(enemySo);
                }
                else
                {
                    Debug.LogError("Invalid goal type for KillingGoal.");
                    isValid = false;
                }

                found = true;
                break;
            }

            if (found) continue;
            Debug.LogError($"Enemy '{value}' not found for goal.");
            isValid = false;
        }

        return isValid;
    }
}
#endif