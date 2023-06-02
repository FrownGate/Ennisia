using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryData : Data
{
    public List<SupportData> Supports;
    [NonSerialized] public Dictionary<string, List<object>> Items;

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

    public List<Gear> GetGears()
    {
        return (List<Gear>)Items["Gears"].Cast<Gear>();
    }

    public Material GetMaterial(int type, int rarity)
    {
        Material material = null;

        foreach (Material item in Items[material.GetType().Name].Cast<Material>())
        {
            if ((int)item.Type == type && (int)item.Rarity == rarity)
            {
                material = item;
                break;
            }
        }

        return material;
    }
}