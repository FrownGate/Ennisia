using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static Item;
using static XPRewardData;

public class XPRewardCSVParserWindow : EditorWindow
{
    private TextAsset _csvFile;

    [MenuItem("Tools/XP Reward CSV Parser")]
    public static void ShowWindow()
    {
        GetWindow<XPRewardCSVParserWindow>("XP Reward CSV Parser");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV File", EditorStyles.boldLabel);
        _csvFile = EditorGUILayout.ObjectField("CSV File", _csvFile, typeof(TextAsset), false) as TextAsset;

        if (GUILayout.Button("Parse CSV Data"))
        {
            if (_csvFile != null)
            {
                XPRewardData xpRewardData = ScriptableObject.CreateInstance<XPRewardData>();
                ParseCSVData(_csvFile, xpRewardData);
                SaveParsedData(xpRewardData);
                Debug.Log("CSV data parsed and saved successfully.");
            }
            else
            {
                Debug.LogError("Please select a CSV file to parse.");
            }
        }
    }

    private void ParseCSVData(TextAsset csvFile, XPRewardData xpRewardData)
    {
        xpRewardData.RewardEntries = new List<RewardEntry>();
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (line.Length > 0)
            {
                string[] parts = line.Split(',');

                if (parts.Length >= 2)
                {
                    int level = int.Parse(parts[0]);
                    string reward = parts[1];

                    List<RewardComponent> rewardComponents = ParseRewardComponents(reward);

                    RewardEntry entry = new RewardEntry
                    {
                        Level = level,
                        Rewards = rewardComponents
                    };

                    xpRewardData.RewardEntries.Add(entry);
                }
            }
        }
    }

    private List<RewardComponent> ParseRewardComponents(string reward)
    {
        List<RewardComponent> rewardComponents = new List<RewardComponent>();

        string[] rewardStrings = reward.Split('+');

        foreach (string rewardString in rewardStrings)
        {
            string trimmedComponent = rewardString.Trim(); // Remove leading and trailing whitespace

            // Split the component into its count and item with rarity parts
            string[] parts = trimmedComponent.Split('x');

            if (parts.Length == 2)
            {
                string countString = parts[1].Trim();
                int count = int.Parse(countString);

                string itemWithRarity = parts[0].Trim();

                // Separate the rarity from the item
                Match match = Regex.Match(itemWithRarity, @"\((.*?)\)");

                string rarity = string.Empty;
                if (match.Success)
                {
                    rarity = match.Groups[1].Value.Trim();
                }

                string item = Regex.Replace(itemWithRarity, @"\s*\(.*?\)", "").Trim();
                if (item.Contains("Summon")) item = item.Replace(" ", "");

                string[] itemParts = item.Split(' ');

                RewardComponent component = new RewardComponent
                {
                    ItemName = item,
                    Count = count,
                    Category = DetermineItemCategory(itemParts[0]),
                    Rarity = DetermineItemRarity(rarity),
                    RewardType = DetermineItemRewardType(itemParts[itemParts.Length - 1])
                };

                rewardComponents.Add(component);
            }
        }

        return rewardComponents;
    }

    private ItemCategory? DetermineItemCategory(string itemName)
    {
        if (Enum.TryParse(itemName, out ItemCategory category))
        {
            return category;
        }

        // Handle unknown item categories or return a default value
        return null;
    }

    private Rarity DetermineItemRarity(string rarityName)
    {
        if (Enum.TryParse(rarityName, out Rarity rarity))
        {
            return rarity;
        }

        // Handle unknown rarities or return a default value
        return Rarity.Common;
    }

    private RewardType DetermineItemRewardType(string RewardTypeName)
    {
        if (Enum.TryParse(RewardTypeName.Trim(), out RewardType type))
        {
            return type;
        }

        return RewardType.None;
    }

    private void SaveParsedData(XPRewardData xpRewardData)
    {
        string savePath = EditorUtility.SaveFilePanelInProject("Save Parsed Data", "XPData", "asset", "Save the parsed XP reward data");

        if (!string.IsNullOrEmpty(savePath))
        {
            AssetDatabase.CreateAsset(xpRewardData, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
