using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryData
{
    //TODO -> Find item by id
    //TODO -> Equip Supports

    public List<SupportData> Supports;
    [NonSerialized] public Dictionary<string, List<Item>> Items;

    public InventoryData()
    {
        Supports = new();
        Items = new();
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

    public bool HasItem(Item itemToFound)
    {
        if (Items.TryGetValue(itemToFound.GetType().Name, out List<Item> items)) {
            return items.Contains(itemToFound);
        }
        return false;
    }

    public Gear GetGearById(int id)
    {
        return (Gear)Items["Gear"].Find(gear => gear.Id == id);
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
        //TODO -> use Find function instead
        Debug.Log($"Looking for item -> Rarity : {rarity} - Type : {type}");

        Item foundItem = null;

        foreach (Item item in Items[itemToGet.GetType().Name])
        {
            if (type != item.Category) continue;
            if (rarity != item.Rarity) continue;

            Debug.Log("Item found !");

            foundItem = item;
            break;
        }

        return foundItem;
    }

    public void RemoveItem(Item itemToRemove)
    {
        if (Items.TryGetValue(itemToRemove.GetType().Name, out List<Item> items))
        {
            items.Remove(itemToRemove);
        }
    }
}