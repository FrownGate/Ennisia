using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XPRewardData", menuName = "Rewards/XP Reward Data")]
public class XPRewardData : ScriptableObject
{
    public enum RewardType
    {
        None, Crystals, Material, SummonTicket, Set, Weapon, Armor, Accessory
    }
    [Serializable]
    public class RewardEntry
    {
        public int Level;
        public List<RewardComponent> Rewards;
    }

    [Serializable]
    public class RewardComponent
    {
        public string ItemName;
        public int Count;
        public Item.ItemCategory Category;
        public Item.ItemRarity Rarity;
        public RewardType RewardType;
    }

    public List<RewardEntry> RewardEntries;

    public void LVLUPReward(int level)
    {
        RewardEntry entry = RewardEntries.Find(x => x.Level == level);
        if (entry == null)
        {
            Debug.LogWarning($"No rewards found for level {level}");
            return;
        }

        foreach (RewardComponent component in entry.Rewards)
        {
            switch (component.RewardType)
            {
                case RewardType.Crystals:
                    PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, component.Count);
                    break;
                case RewardType.Material:
                    PlayFabManager.Instance.AddInventoryItem(new Material(component.Category, component.Rarity, component.Count));
                    break;
                case RewardType.SummonTicket:
                    SummonTicket summonTicket = new(component.Rarity, component.Count);
                    PlayFabManager.Instance.AddInventoryItem(summonTicket);
                    break;
                case RewardType.Armor:
                    Item.GearType armorType = (Item.GearType)UnityEngine.Random.Range(0, 3);
                    Gear armor = new(armorType, component.Rarity);
                    PlayFabManager.Instance.AddInventoryItem(armor);
                    break;
                case RewardType.Accessory:
                    Item.GearType accessoryType = (Item.GearType)UnityEngine.Random.Range(3, 6);
                    Gear accessory = new(accessoryType, component.Rarity);
                    PlayFabManager.Instance.AddInventoryItem(accessory);
                    break;
                case RewardType.Weapon:
                    GearSO[] weaponSO = Resources.LoadAll<GearSO>("SO/Weapons");
                    int randomIndex = UnityEngine.Random.Range(0, weaponSO.Length);
                    Gear weapon = new(weaponSO[randomIndex], component.Rarity);
                    PlayFabManager.Instance.AddInventoryItem(weapon);
                    break;
                case RewardType.Set:
                    
                    Item.GearSet randomSet = (Item.GearSet)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Item.GearSet)).Length);

                    List<Gear> set = new()
                    {
                        new(Item.GearType.Helmet, component.Rarity),
                        new(Item.GearType.Chest, component.Rarity),
                        new(Item.GearType.Boots, component.Rarity),
                        new(Item.GearType.Earrings, component.Rarity),
                        new(Item.GearType.Ring, component.Rarity),
                        new(Item.GearType.Necklace, component.Rarity)
                    };

                    foreach (var gear in set)
                    {
                        gear.Set = randomSet;
                        PlayFabManager.Instance.AddInventoryItem(gear);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
