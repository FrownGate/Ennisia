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

        if (!GUILayout.Button("Parse CSV Data")) return;
        if (_csvFile != null)
        {
            var xpRewardData = CreateInstance<XPRewardData>();
            ParseCSVData(_csvFile, xpRewardData);
            SaveParsedData(xpRewardData);
            Debug.Log("CSV data parsed and saved successfully.");
        }
        else
        {
            Debug.LogError("Please select a CSV file to parse.");
        }
    }

    private void ParseCSVData(TextAsset csvFile, XPRewardData xpRewardData)
    {
        xpRewardData.RewardEntries = new List<RewardEntry>();
        var lines = csvFile.text.Split('\n');

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();

            if (line.Length <= 0) continue;
            var parts = line.Split(',');

            if (parts.Length < 2) continue;
            var level = int.Parse(parts[0]);
            var reward = parts[1];

            var rewardComponents = ParseRewardComponents(reward);

            var entry = new RewardEntry
            {
                Level = level,
                Rewards = rewardComponents
            };

            xpRewardData.RewardEntries.Add(entry);
        }
    }

    private List<RewardComponent> ParseRewardComponents(string reward)
    {
        var rewardComponents = new List<RewardComponent>();

        var rewardStrings = reward.Split('+');

        foreach (var rewardString in rewardStrings)
        {
            var trimmedComponent = rewardString.Trim(); // Remove leading and trailing whitespace

            // Split the component into its count and item with rarity parts
            var parts = trimmedComponent.Split('x');

            if (parts.Length != 2) continue;
            var countString = parts[1].Trim();
            var count = int.Parse(countString);

            var itemWithRarity = parts[0].Trim();

            // Separate the rarity from the item
            var match = Regex.Match(itemWithRarity, @"\((.*?)\)");

            var rarity = string.Empty;
            if (match.Success) rarity = match.Groups[1].Value.Trim();

            var item = Regex.Replace(itemWithRarity, @"\s*\(.*?\)", "").Trim();
            if (item.Contains("Summon")) item = item.Replace(" ", "");

            var itemParts = item.Split(' ');

            var component = new RewardComponent
            {
                ItemName = item,
                Count = count,
                Category = DetermineItemCategory(itemParts[0]),
                Rarity = DetermineItemRarity(rarity),
                RewardType = DetermineItemRewardType(itemParts[^1])
            };

            rewardComponents.Add(component);
        }

        return rewardComponents;
    }

    private ItemCategory DetermineItemCategory(string itemName)
    {
        return Enum.TryParse(itemName, out ItemCategory category) ? category :
            // Handle unknown item categories or return a default value
            ItemCategory.Accessory;
    }

    private Rarity DetermineItemRarity(string rarityName)
    {
        return Enum.TryParse(rarityName, out Rarity rarity) ? rarity :
            // Handle unknown rarities or return a default value
            Rarity.Common;
    }

    private RewardType DetermineItemRewardType(string RewardTypeName)
    {
        return Enum.TryParse(RewardTypeName.Trim(), out RewardType type) ? type : RewardType.None;
    }

    private void SaveParsedData(XPRewardData xpRewardData)
    {
        var savePath =
            EditorUtility.SaveFilePanelInProject("Save Parsed Data", "XPData", "asset",
                "Save the parsed XP reward data");

        if (string.IsNullOrEmpty(savePath)) return;
        AssetDatabase.CreateAsset(xpRewardData, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}