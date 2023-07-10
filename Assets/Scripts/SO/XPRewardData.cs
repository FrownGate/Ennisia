using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XPRewardData", menuName = "Rewards/XP Reward Data")]
public class XPRewardData : ScriptableObject
{
    public enum RewardType
    {
        None,
        Crystals,
        Material,
        SummonTicket,
        Set,
        Weapon,
        Armor,
        Accessory
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
        public ItemCategory Category;
        public Rarity Rarity;
        public RewardType RewardType;
    }

    public List<RewardEntry> RewardEntries;

    public void LvlupReward(int level)
    {
        var entry = RewardEntries.Find(x => x.Level == level);
        if (entry == null)
        {
            Debug.LogWarning($"No rewards found for level {level}");
            return;
        }

        foreach (var component in entry.Rewards)
            switch (component.RewardType)
            {
                case RewardType.Crystals:
                    PlayFabManager.Instance.AddCurrency(Currency.Crystals, component.Count);
                    break;
                case RewardType.Material:
                    PlayFabManager.Instance.AddInventoryItem(new Material((ItemCategory)component.Category,
                        component.Rarity, component.Count));
                    break;
                case RewardType.SummonTicket:
                    SummonTicket summonTicket = new(component.Rarity, component.Count);
                    PlayFabManager.Instance.AddInventoryItem(summonTicket);
                    break;
                case RewardType.Armor:
                    var armorType = (GearType)UnityEngine.Random.Range(0, 3);
                    Gear armor = new(armorType, component.Rarity, null);
                    PlayFabManager.Instance.AddInventoryItem(armor);
                    break;
                case RewardType.Accessory:
                    var accessoryType = (GearType)UnityEngine.Random.Range(3, 6);
                    Gear accessory = new(accessoryType, component.Rarity, null);
                    PlayFabManager.Instance.AddInventoryItem(accessory);
                    break;
                case RewardType.Weapon:
                    var weaponSO = Resources.LoadAll<GearSO>("SO/Weapons");
                    var randomIndex = UnityEngine.Random.Range(0, weaponSO.Length);
                    Gear weapon = new(weaponSO[randomIndex], component.Rarity);
                    PlayFabManager.Instance.AddInventoryItem(weapon);
                    break;
                case RewardType.Set:

                    var randomSet = (GearSetData)UnityEngine.Random.Range(0, Enum.GetValues(typeof(GearSet)).Length);

                    List<Gear> set = new()
                    {
                        new Gear(GearType.Helmet, component.Rarity, null),
                        new Gear(GearType.Chest, component.Rarity, null),
                        new Gear(GearType.Boots, component.Rarity, null),
                        new Gear(GearType.Earrings, component.Rarity, null),
                        new Gear(GearType.Ring, component.Rarity, null),
                        new Gear(GearType.Necklace, component.Rarity, null)
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