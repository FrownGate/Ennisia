using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Item
{
    [NonSerialized] public int Id;
    [NonSerialized] public string Icon; //TODO -> create function to return icon path with name
    public string Type;
    public string Attribute; //TODO -> save attributes as int in csv and add function to return attribute name
    public float Value;
    public string Description;

    public Gear(string type, int rarity)
    {
        Id = PlayFabManager.Instance.Inventory.GetGears().Count + 1;
        Type = type;
        Rarity = (ItemRarity)rarity;
        Description = "";
        Attribute = SetAttribute();
        Value = SetValue();
        Stack = Id.ToString();

        AddToInventory();
    }

    public Gear(InventoryItem item)
    {
        Gear gear = JsonUtility.FromJson<Gear>(item.DisplayProperties.ToString());

        Id = int.Parse(item.StackId);
        Stack = item.StackId;
        Type = gear.Type;
        Rarity = gear.Rarity;
        Attribute = gear.Attribute;
        Value = gear.Value;

        AddToInventory();
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

    protected override void SetName()
    {
        Name = $"[{Rarity}] {Type}";
    }
}