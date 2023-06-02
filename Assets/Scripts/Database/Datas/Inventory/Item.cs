using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemRarity
    {
        Null, Common, Rare, Epic, Legendary
    }

    public enum MaterialType
    {
        Null, Weapon, Gear, Accessory
    }

    [NonSerialized] public string Stack;
    [NonSerialized] public string Name;
    [NonSerialized] public int Amount; //Amount of item to add
    public ItemRarity Rarity;
    public MaterialType Type;

    //TODO -> Remove item if amount == 0

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