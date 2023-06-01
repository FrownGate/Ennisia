using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : Data
{
    public List<SupportData> Supports;
    [NonSerialized] public List<Gear> Gears;
    [NonSerialized] public List<Material> Materials;
    [NonSerialized] public List<SummonTicket> SummonTickets;
    //TODO -> replace with dicitonary of list object ?

    public InventoryData()
    {
        ClassName = "Inventory"; //TODO -> replace with GetType name
        Supports = new();
        Gears = new();
        Materials = new();
        SummonTickets = new();
    }

    public override void UpdateLocalData(string json)
    {
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);
        Supports = data.Supports;
    }

    public Material GetMaterial(int type, int rarity)
    {
        Material material = null;

        foreach (Material item in Materials)
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