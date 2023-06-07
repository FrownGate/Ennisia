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
        Helmet, Chest, Boots, Earrings, Necklace, Ring,
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
    public AttributeStat? Attribute;

    //Json Utility
    public string JsonRarity;
    public string JsonCategory;
    public string JsonType;
    public string JsonAttribute;

    //TODO -> Remove item if amount == 0

    protected void AddToInventory()
    {
        Dictionary<string, List<Item>> inventory = PlayFabManager.Instance.Data.Inventory.Items;
        SetName();

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
        JsonAttribute = Attribute.ToString();
    }

    public virtual void Deserialize()
    {
        Rarity = string.IsNullOrEmpty(JsonRarity) ? null : Enum.Parse<ItemRarity>(JsonRarity);
        Category = string.IsNullOrEmpty(JsonCategory) ? null : Enum.Parse<ItemCategory>(JsonCategory);
        Type = string.IsNullOrEmpty(JsonType) ? null : Enum.Parse<GearType>(JsonType);
        Attribute = string.IsNullOrEmpty(JsonAttribute) ? null : Enum.Parse<AttributeStat>(JsonAttribute);
    }

    protected virtual void SetName() { }
    public virtual void Upgrade() { }
}