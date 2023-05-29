using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GearData
{
    public int Id;
    public string Name;
    public string Type;
    public string Rarity;
    public string Attribute;
    public float Value;
    public string Description;
    public string Icon;

    public GearData() { }

    public GearData(string type, string rarity, int id)
    {
        Id = id;
        Name = $"[{rarity}] {type}";
        Type = type;
        Rarity = rarity;
        Description = "";
    }

    public GearData(InventoryItem item)
    {
        Id = int.Parse(item.StackId);
        //Name = name;
        //Type = type;
        //Rarity = rarity;
        //Attribute = attribute;
        //Value = value;
        //Description = description;
        //Icon = icon;
    }

    private int SetAttribute()
    {
        return 0;
    }

    private int SetValue()
    {
        return 0;
    }

    public bool IsValid()
    {
        bool isValid = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Type) && CheckRarity() && !string.IsNullOrEmpty(Attribute) && Value > 0f && string.IsNullOrEmpty(Description) & !string.IsNullOrEmpty(Icon);
        if (!isValid) Debug.LogError("Gear isn't valid !");
        return isValid;
    }

    private bool CheckRarity()
    {
        string[] rarities = new string[3] { "Legendary", "Epic", "Rare" };
        return rarities.Contains(Rarity);
    }
}