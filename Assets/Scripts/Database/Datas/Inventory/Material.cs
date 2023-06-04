using PlayFab.EconomyModels;
using UnityEngine;

public class Material : Item
{
    public Material() { }

    public Material(ItemCategory category, ItemRarity rarity, int amount = 1)
    {
        Category = category;
        Rarity = rarity;
        Stack = $"{Category}_{Rarity}";
        Amount = amount;

        AddToInventory();
    }

    public Material(InventoryItem item)
    {
        Material material = JsonUtility.FromJson<Material>(item.DisplayProperties.ToString());

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Category = material.Category;
        Rarity = material.Rarity;

        AddToInventory();
    }

    protected override void SetName()
    {
        Name = $"[{Rarity}] {Category} {GetType().Name}";
    }
}