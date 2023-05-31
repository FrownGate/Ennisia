using PlayFab.EconomyModels;
using System;
using UnityEngine;

public class Material : Item
{
    public enum MaterialType
    {
        Weapon, Gear, Accessory
    }

    [NonSerialized] public string Name;
    [NonSerialized] public string Stack;
    [NonSerialized] public int Amount;
    public MaterialType Type;

    public Material(int type, int rarity)
    {
        Stack = $"{Type}_{Rarity}";
        Type = (MaterialType)type;
        Rarity = (ItemRarity)rarity;
        Name = SetName();
    }

    public Material(InventoryItem item)
    {
        Material material = JsonUtility.FromJson<Material>(item.DisplayProperties.ToString());

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Type = material.Type;
        Rarity = material.Rarity;
        Name = SetName();

        PlayFabManager.Instance.Inventory.Materials.Add(this);
        Debug.Log($"Getting {Name} item !");
    }

    private string SetName()
    {
        return $"[{Type}] {Rarity} {GetType().Name}";
    }
}