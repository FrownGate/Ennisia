using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Item
{
    [NonSerialized] public int Id;
    [NonSerialized] public string Icon; //TODO -> create function to return icon path with name
    public float Value;
    public string Description;

    public Gear(GearType type, ItemRarity rarity)
    {
        Id = SetId();
        Type = type;
        Rarity = rarity;
        Description = "";
        Attribute = SetAttribute();
        Value = SetValue();
        Stack = Id.ToString();
        Amount = 1;
        //TODO -> Set Category function

        AddToInventory();
    }

    public Gear(InventoryItem item)
    {
        Gear gear = JsonUtility.FromJson<Gear>(item.DisplayProperties.ToString());
        gear.Deserialize();

        Id = int.Parse(item.StackId);
        Stack = item.StackId;
        Type = gear.Type;
        Rarity = gear.Rarity;
        Attribute = gear.Attribute;
        Value = gear.Value;

        AddToInventory();
    }

    private int SetId()
    {
        if (PlayFabManager.Instance.Inventory.Items.ContainsKey("Gear"))
        {
            return PlayFabManager.Instance.Inventory.Items["Gear"].Count + 1;
        }
        else
        {
            return 1;
        }
    }

    private AttributeStat SetAttribute()
    {
        //TODO -> save attributes as int in csv and add function to return attribute name
        List<AttributeStat> possiblesAttributes = Resources.Load<EquipmentAttributesSO>($"SO/EquipmentStats/Attributes/{Type}").Attributes;
        return possiblesAttributes[UnityEngine.Random.Range(0, possiblesAttributes.Count - 1)];
    }

    private int SetValue()
    {
        StatMinMaxValuesSO possibleValues = Resources.Load<StatMinMaxValuesSO>($"SO/EquipmentStats/Values/{Type}_{Rarity}_{Attribute}");
        return UnityEngine.Random.Range(possibleValues.MinValue, possibleValues.MaxValue);
    }

    protected override void SetName()
    {
        Name = $"[{Rarity}] {Type}";
    }
}