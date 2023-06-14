using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemRarity
    {
        Common, Rare, Epic, Legendary
    }

    public enum ItemCategory
    {
        Weapon, Armor, Accessory
    }

    public enum GearType
    {
        Helmet, Chest, Boots, Earrings, Necklace, Ring, Weapon
    }

    public enum GearWeaponType
    {
        Sword, Staff, Scythe, Daggers, Hammer, Shield, Bow
    }

    public enum AttributeStat
    {
        PhysicalDamages, MagicalDamages, Attack, HP, Defense, CritRate, CritDmg, Speed
    }

    public string Stack;
    public string Name;
    public int Amount; //Amount of item to add
    public ItemRarity? Rarity;
    public ItemCategory? Category;
    public GearType? Type;
    public GearWeaponType? WeaponType;
    public AttributeStat? Attribute;

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

        Debug.Log($"Adding {Name} to inventory...");

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
        Rarity = string.IsNullOrEmpty(JsonRarity) ? null : Enum.Parse<ItemRarity>(JsonRarity);
        Category = string.IsNullOrEmpty(JsonCategory) ? null : Enum.Parse<ItemCategory>(JsonCategory);
        Type = string.IsNullOrEmpty(JsonType) ? null : Enum.Parse<GearType>(JsonType);
        WeaponType = string.IsNullOrEmpty(JsonWeapon) ? null : Enum.Parse<GearWeaponType>(JsonWeapon);
        Attribute = string.IsNullOrEmpty(JsonAttribute) ? null : Enum.Parse<AttributeStat>(JsonAttribute);
    }

    protected virtual void SetName() { }
    public virtual void Upgrade() { }
}