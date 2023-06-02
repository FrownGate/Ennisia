using PlayFab.EconomyModels;
using System;
using UnityEngine;

public class Material : Item
{
    public enum MaterialType
    {
        Weapon, Gear, Accessory
    }

    public MaterialType Type;

    public Material(int type, int rarity, int amount = 1)
    {
        Type = (MaterialType)type;
        Rarity = (ItemRarity)rarity;
        Stack = $"{Type}_{Rarity}";
        Amount = amount;

        AddToInventory();
    }

    public Material(InventoryItem item)
    {
        Material material = JsonUtility.FromJson<Material>(item.DisplayProperties.ToString());

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Type = material.Type;
        Rarity = material.Rarity;

        AddToInventory();
    }

    protected override void SetName()
    {
        Name = $"[{Rarity}] {Type} {GetType().Name}";
    }
}