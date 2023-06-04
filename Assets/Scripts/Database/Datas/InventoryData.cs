using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryData : Data
{
    public List<SupportData> Supports;
    [NonSerialized] public Dictionary<string, List<Item>> Items;

    public InventoryData()
    {
        Supports = new();
        Items = new();
    }

    public override void UpdateLocalData(string json)
    {
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);
        Supports = data.Supports;
    }

    public Material GetMaterial(int type, int rarity)
    {
        Material material = null;

        foreach (Material item in Items[material.GetType().Name])
        {
            if ((int)item.Category == type && (int)item.Rarity == rarity)
            {
                material = item;
                break;
            }
        }

        return material;
    }

    public bool HasItem(Item item)
    {
        return Items.ContainsKey(item.GetType().Name);
    }

    public Item GetItem(Item itemToGet, Item.ItemCategory type)
    {
        return GetItem(itemToGet, type, null);
    }

    public Item GetItem(Item itemToGet, Item.ItemRarity rarity)
    {
        return GetItem(itemToGet, null, rarity);
    }

    //Get an item with corresponding Type and/or Rarity
    public Item GetItem(Item itemToGet, Item.ItemCategory? type, Item.ItemRarity? rarity)
    {
        Debug.Log($"Looking for item -> Rarity : {rarity} - Type : {type}");

        Item foundItem = null;

        foreach (Item item in Items[itemToGet.GetType().Name])
        {
            if (type != null && type != item.Category) continue;
            if (rarity != null && rarity != item.Rarity) continue;

            Debug.Log("Item found !");

            foundItem = item;
            break;
        }

        return foundItem;
    }
}