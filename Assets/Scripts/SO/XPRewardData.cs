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
        public ItemCategory? Category;
        public Rarity Rarity;
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
                    PlayFabManager.Instance.AddCurrency(Currency.Crystals, component.Count);
                    break;
                case RewardType.Material:
                    PlayFabManager.Instance.AddInventoryItem(new Material((ItemCategory)component.Category, component.Rarity, component.Count));
                    break;
                case RewardType.SummonTicket:
                    SummonTicket summonTicket = new(component.Rarity, component.Count);
                    PlayFabManager.Instance.AddInventoryItem(summonTicket);
                    break;
                case RewardType.Armor:
                    GearType armorType = (GearType)UnityEngine.Random.Range(0, 3);
                    Gear armor = new(armorType, component.Rarity);
                    PlayFabManager.Instance.AddInventoryItem(armor);
                    break;
                case RewardType.Accessory:
                    GearType accessoryType = (GearType)UnityEngine.Random.Range(3, 6);
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

                    GearSetData randomSet = (GearSetData)UnityEngine.Random.Range(0, Enum.GetValues(typeof(GearSet)).Length);

                    List<Gear> set = new()
                    {
                        new(GearType.Helmet, component.Rarity),
                        new(GearType.Chest, component.Rarity),
                        new(GearType.Boots, component.Rarity),
                        new(GearType.Earrings, component.Rarity),
                        new(GearType.Ring, component.Rarity),
                        new(GearType.Necklace, component.Rarity)
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