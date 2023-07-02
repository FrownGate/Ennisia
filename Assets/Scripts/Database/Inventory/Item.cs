using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Weapon, Armor, Accessory
}

public enum GearType
{
    Helmet, Chest, Boots, Earrings, Necklace, Ring, Weapon
}

public enum WeaponType
{
    Sword, Staff, Scythe, Daggers, Hammer, Shield, Bow, Gloves
}

public enum GearSetData //TODO -> Use individual classes instead
{
    FightUntilTheEnd, WarriorWill, PowerOfTheSorcerer, Sage, LuckyPull,
    Executioner, Revitalise, ArmorOfTheDead, Herald, FairyTales,
    AccuracyDevice, DemonsAndHumans, VitalEngagement, MagicShield, IntegralArmour
}

[Serializable]
public class Item
{
    public int Id;
    public string Stack;
    public string Name;
    public int Amount; //Amount of item to add
    public Rarity? Rarity;
    public ItemCategory? Category;
    public GearType? Type;
    public WeaponType? WeaponType;
    public Attribute? Attribute;
    public GearSetData? Set;

    //Json Utility
    public string JsonRarity;
    public string JsonCategory;
    public string JsonType;
    public string JsonWeapon;
    public string JsonAttribute;

    protected void AddToInventory()
    {
        Dictionary<string, List<Item>> inventory = PlayFabManager.Instance.Data.Inventory.Items;
        if (string.IsNullOrEmpty(Name)) SetName();

        //Debug.Log($"Adding {Name} to inventory...");

        if (!inventory.ContainsKey(GetType().Name))
        {
            inventory[GetType().Name] = new() { this };
        }
        else
        {
            foreach (Item item in inventory[GetType().Name])
            {
                if (item.Stack == Stack)
                {
                    item.Amount = Amount;
                    return;
                }
            }

            inventory[GetType().Name].Add(this);
        }
    }

    public virtual void Serialize()
    {
        JsonRarity = Rarity.ToString();
        JsonCategory = Category.ToString();
        JsonType = Type.ToString();
        JsonWeapon = WeaponType.ToString();
        JsonAttribute = Attribute.ToString();
    }

    public virtual void Deserialize()
    {
        Rarity = string.IsNullOrEmpty(JsonRarity) ? null : Enum.Parse<Rarity>(JsonRarity);
        Category = string.IsNullOrEmpty(JsonCategory) ? null : Enum.Parse<ItemCategory>(JsonCategory);
        Type = string.IsNullOrEmpty(JsonType) ? null : Enum.Parse<GearType>(JsonType);
        WeaponType = string.IsNullOrEmpty(JsonWeapon) ? null : Enum.Parse<WeaponType>(JsonWeapon);
        Attribute = string.IsNullOrEmpty(JsonAttribute) ? null : Enum.Parse<Attribute>(JsonAttribute);
    }

    protected virtual void SetName() { }
    public virtual void Upgrade() { }
}