using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Gear
{
    [NonSerialized] public int Id;
    [NonSerialized] public string Icon; //TODO -> create function to return icon path with name
    public string Name;
    public string Type;
    public string Rarity;
    public string Attribute; //TODO -> save attributes as int in csv and add function to return attribute name
    public float Value;
    public string Description;

    public Gear(string type, string rarity, int id)
    {
        Id = id;
        Type = type;
        Rarity = rarity;
        Description = "";
        Attribute = SetAttribute();
        Value = SetValue();
        Name = $"[{rarity}] {type}";
    }

    public Gear(InventoryItem item)
    {
        Gear gear = JsonUtility.FromJson<Gear>(item.DisplayProperties.ToString());

        Id = int.Parse(item.StackId);
        Name = gear.Name;
        Type = gear.Type;
        Rarity = gear.Rarity;
        Attribute = gear.Attribute;
        Value = gear.Value;

        PlayFabManager.Instance.Inventory.Gears.Add(this);
        Debug.Log($"Getting {Name} item !");
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