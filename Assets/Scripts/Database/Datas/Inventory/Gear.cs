using PlayFab.EconomyModels;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Item
{
    public int Id;
    public string Icon; //TODO -> create function to return icon path with name
    public float Value;
    public string Description;
    public int Level;
    public float StatUpgrade;
    public float RatioUpgrade;

    public Gear(GearType type, ItemRarity rarity)
    {
        Id = SetId();
        Stack = Id.ToString();
        Type = type;
        Rarity = rarity;
        Description = "";
        Category = SetCategory();
        Attribute = SetAttribute();
        Value = SetValue();
        Level = 1;
        Amount = 1;

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
        Description = gear.Description;
        Category = gear.Category;
        Attribute = gear.Attribute;
        Value = gear.Value;
        Level = gear.Level;
        Amount = gear.Amount;

        AddToInventory();
    }

    private int SetId()
    {
        if (PlayFabManager.Instance.Data.Inventory.Items.ContainsKey("Gear"))
        {
            return PlayFabManager.Instance.Data.Inventory.Items["Gear"].Count + 1;
        }
        else
        {
            return 1;
        }
    }

    private ItemCategory SetCategory()
    {
        if ((int)Type < 3) return ItemCategory.Armor;
        else if ((int)Type < 6) return ItemCategory.Accessory;
        else return ItemCategory.Weapon;
    }

    private AttributeStat SetAttribute()
    {
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

    public override void Upgrade()
    {
        if (Level >= 50) return;
        Debug.Log($"Upgrading {Name}...");

        Level++;
        Value += (StatUpgrade * Level) + (Value * RatioUpgrade * Level);

        PlayFabManager.Instance.UpdateItem(this);
    }

    public void Upgrade(int _level) //Used ?
    {
        if (_level >= 50) return;
        Debug.Log($"Upgrading {Name}...");

        Level++;
        Value += (StatUpgrade * _level) + (Value * RatioUpgrade * _level);

        PlayFabManager.Instance.UpdateItem(this);
    }

    public void Equip()
    {
        //TODO -> Set EquippedGear SO datas
    }
}