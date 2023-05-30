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
    public string Attribute; //TODO -> save attributes as int in csv and add function to return attribute name
    public float Value;
    public string Description;
    public string Icon; //TODO -> create function to return icon path with name

    public GearData() { }

    public GearData(string type, string rarity, int id)
    {
        Id = id;
        Type = type;
        Rarity = rarity;
        Description = "";
        Attribute = SetAttribute();
        Value = SetValue();
        Name = $"[{rarity}] {type}";
    }

    private string SetAttribute()
    {
        List<string> possiblesAttributes = Resources.Load<EquipmentAttributeSO>($"SO/EquipmentStats/Attributes/{Type}").Attributes;
        return possiblesAttributes[UnityEngine.Random.Range(0, possiblesAttributes.Count - 1)];
    }

    private int SetValue()
    {
        EquipmentValueSO possibleValues = Resources.Load<EquipmentValueSO>($"SO/EquipmentStats/Values/{Type}_{Rarity}_{Attribute}");
        return UnityEngine.Random.Range(possibleValues.MinValue, possibleValues.MaxValue);
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