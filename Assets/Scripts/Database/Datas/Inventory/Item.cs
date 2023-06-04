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

    [NonSerialized] public string Stack;
    [NonSerialized] public string Name;
    [NonSerialized] public int Amount; //Amount of item to add
    public ItemRarity? Rarity;
    public ItemCategory? Category;
    public GearType? Type;
    public AttributeStat? Attribute;

    //TODO -> Remove item if amount == 0

    public Item()
    {
        Rarity = null;
        Category = null;
        Debug.Log(Attribute);
    }

    protected void AddToInventory()
    {
        Dictionary<string, List<Item>> inventory = PlayFabManager.Instance.Inventory.Items;
        SetName();

        Debug.Log($"Adding {Name} to inventory !");

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

    protected virtual void SetName() { }
}