using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using static Item;
using static UnityEditor.Progress;
using static XPRewardData;

public class XPRewardCSVParser : MonoBehaviour
{
    public XPRewardData xpRewardData;
    public TextAsset csvFile;

    public void ParseCSVData()
    {
        xpRewardData.rewardEntries = new();
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
                        level = level,
                        rewards = rewardComponents
                    };

                    xpRewardData.rewardEntries.Add(entry);
                }
            }
        }
    }

    private List<RewardComponent> ParseRewardComponents(string reward)
    {
        List<RewardComponent> rewardComponents = new();

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
                    itemName = item,
                    count = count,
                    category = DetermineItemCategory(itemParts[0]),
                    rarity = DetermineItemRarity(rarity),
                    rewardType = DetermineItemRewardType(itemParts[itemParts.Length - 1])
                };

                rewardComponents.Add(component);
            }
        }

        return rewardComponents;
    }

    private ItemCategory DetermineItemCategory(string itemName)
    {
        if (Enum.TryParse(itemName, out ItemCategory category))
        {
            return category;
        }

        // Handle unknown item categories or return a default value
        return ItemCategory.None;
    }

    private ItemRarity DetermineItemRarity(string rarityName)
    {
        if (Enum.TryParse(rarityName, out ItemRarity rarity))
        {
            return rarity;
        }

        // Handle unknown rarities or return a default value
        return ItemRarity.Common;
    }
    private RewardType DetermineItemRewardType(string RewardTypeName)
    {

        if (Enum.TryParse(RewardTypeName.Trim(), out RewardType type))
        {
            return type;
        }
        return RewardType.None;
    }
}
