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
        public int level;
        public List<RewardComponent> rewards;
    }

    [Serializable]
    public class RewardComponent
    {
        public string itemName;
        public int count;
        public Item.ItemCategory category;
        public Item.ItemRarity rarity;
        public RewardType rewardType;
    }

    public List<RewardEntry> rewardEntries;

    public void LVLUPReward(int level)
    {
        RewardEntry entry = rewardEntries.Find(x => x.level == level);
        if (entry == null)
        {
            Debug.LogWarning($"No rewards found for level {level}");
            return;
        }

        foreach (RewardComponent component in entry.rewards)
        {
            switch (component.rewardType)
            {
                case RewardType.Crystals:
                    PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, component.count);
                    break;
                case RewardType.Material:
                    PlayFabManager.Instance.AddInventoryItem(new Material(component.category, component.rarity, component.count));
                    break;
                case RewardType.SummonTicket:
                    SummonTicket summonTicket = new(component.rarity, component.count);
                    PlayFabManager.Instance.AddInventoryItem(summonTicket);
                    break;
                case RewardType.Armor:
                    Gear.GearType armorType = (Item.GearType)UnityEngine.Random.Range(0, 3);
                    Gear armor = new Gear(armorType, component.rarity);
                    PlayFabManager.Instance.AddInventoryItem(armor);
                    break;
                case RewardType.Accessory:
                    Gear.GearType accessoryType = (Item.GearType)UnityEngine.Random.Range(3, 6);
                    Gear accessory = new(accessoryType, component.rarity);
                    PlayFabManager.Instance.AddInventoryItem(accessory);
                    break;
                case RewardType.Weapon:
                    GearSO[] weaponSO = Resources.LoadAll<GearSO>("SO/Weapons");
                    int randomIndex = UnityEngine.Random.Range(0, weaponSO.Length);
                    Gear weapon = new(weaponSO[randomIndex], component.rarity);
                    PlayFabManager.Instance.AddInventoryItem(weapon);
                    break;
                case RewardType.Set:
                    // TODO -> Handle set reward
                    break;
                default:
                    break;
            }
        }
    }
}
