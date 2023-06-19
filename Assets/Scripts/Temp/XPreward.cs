using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class XPreward : MonoBehaviour
{
    public enum RewardType
    {
        None, Crystal, Material, SummonTicket, Set, Weapon
    }

    private Dictionary<int, string> rewards;
    string csvFilePath = "Assets/Resources/CSV/XpCSV-AccountReward.csv";
    public void LoadRewardsFromCSV()
    {
        rewards = new();
        string[] lines = File.ReadAllLines(csvFilePath);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string[] parts = line.Split(',');

            if (parts.Length == 2)
            {
                int level = int.Parse(parts[0]);
                string reward = parts[1];

                rewards[level] = reward;
            }
        }
    }
    public void LVLUPReward(int level)
    {
        if(rewards == null)
        {
            LoadRewardsFromCSV();
        }
        string reward = rewards[level];

        string[] rewardComponents = reward.Split('+');

        foreach (string component in rewardComponents)
        {
            string trimmedComponent = component.Trim(); // Remove leading and trailing whitespace

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
                string[] itemParts = item.Split(' ');
                // Determine the item category based on the item name (modify as needed)
                Item.ItemCategory category = DetermineItemCategory(itemParts[0].Trim());

                // Determine the item rarity based on the rarity name (modify as needed)
                Item.ItemRarity itemRarity = DetermineItemRarity(rarity);
                if (itemParts.Length > 1)
                {
                    if (Enum.TryParse(itemParts[1].Trim(), out RewardType type))
                    {
                        switch (type)
                        {
                            case RewardType.None:
                                break;
                            case RewardType.Material:
                                PlayFabManager.Instance.AddInventoryItem(new Material(category, itemRarity, count));
                                break;
                            case RewardType.SummonTicket:
                                break;
                            case RewardType.Set:
                                break;
                            case RewardType.Weapon:
                                break;
                            default:
                                break;
                        }

                    }
                }
                else
                {
                    PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, count);
                }
                // Add the inventory item

            }
            else
            {
                // Handle cases where the count is not specified, if needed
            }
        }
    }

    // Function to determine the item category based on the item name
    Item.ItemCategory DetermineItemCategory(string itemName)
    {
        // Modify this function based on your item categorization logic
        // Example implementation:

        if (Enum.TryParse(itemName, out Item.ItemCategory category))
        {
            return category;
        }

        // Handle unknown item categories or return a default value
        return Item.ItemCategory.None;
    }

    // Function to determine the item rarity based on the rarity name
    Item.ItemRarity DetermineItemRarity(string rarityName)
    {
        // Modify this function based on your rarity categorization logic
        // Example implementation:

        if (Enum.TryParse(rarityName, out Item.ItemRarity rarity))
        {
            return rarity;
        }

        // Handle unknown rarities or return a default value
        return Item.ItemRarity.None;
    }


}
