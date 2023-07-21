using PlayFab.EconomyModels;
using UnityEngine;

public class Material : Item
{
    public Material() { }

    public Material(ItemCategory category, Rarity rarity, int amount = 1)
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
        material.Deserialize();

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Category = material.Category;
        Rarity = material.Rarity;
        Name = material.Name;

        AddToInventory();
    }

    public Material(CatalogItem item)
    {
        Material material = JsonUtility.FromJson<Material>(item.DisplayProperties.ToString());
        material.Deserialize();

        Stack = item.Id;
        Amount = 1;
        Category = material.Category;
        Rarity = material.Rarity;
        Name = material.Name;

        AddToInventory();
    }

    protected override void SetName()
    {
        Name = $"[{Rarity}] {Category} {GetType().Name}";
    }
}