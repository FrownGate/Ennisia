using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemRarity
    {
        Common, Rare, Epic, Legendary
    }

    [NonSerialized] public string Stack;
    [NonSerialized] public string Name;
    [NonSerialized] public int Amount; //Amount of item to add
    public ItemRarity Rarity;

    //TODO -> Remove item if amount == 0

    protected void AddToInventory()
    {
        Dictionary<string, List<object>> inventory = PlayFabManager.Instance.Inventory.Items;
        SetName();

        Debug.Log($"Adding {Name} to inventory !");

        if (!inventory.ContainsKey(GetType().Name))
        {
            inventory[GetType().Name] = new() { this };
        }
        else
        {
            foreach (Item item in inventory[GetType().Name].Cast<Item>())
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