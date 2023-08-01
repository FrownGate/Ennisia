using PlayFab.EconomyModels;
using UnityEngine;

public class Bundle : Item
{
    public Bundle() { }

    public Bundle(ItemCategory category, Rarity rarity, int amount = 1)
    {
        Category = category;
        Rarity = rarity;
        Stack = $"{Category}_{Rarity}";
        Amount = amount;

        AddToInventory();
    }

    public Bundle(InventoryItem item)
    {
        Bundle bundle = JsonUtility.FromJson<Bundle>(item.DisplayProperties.ToString());
        bundle.Deserialize();

        // Stack = item.StackId;
        // Amount = (int)item.Amount;
        IdString = item.Id;
        Category = bundle.Category;
        Rarity = bundle.Rarity;
        Name = bundle.Name;
        Available = bundle.Available;

    }

    public Bundle(CatalogItem item)
    {
        Bundle bundle = JsonUtility.FromJson<Bundle>(item.DisplayProperties.ToString());
        bundle.Deserialize();

        // Stack = item.Id;
        // Amount = 1;

        IdString = item.Id;
        Category = bundle.Category;
        Rarity = bundle.Rarity;
        Name = bundle.Name;
        Available = bundle.Available;

    }

    // public override void Serialize()
    // {
    //     if (Category != ItemCategory.Weapon && Rarity != global::Rarity.Common)
    //     {
    //         JsonSubstats = new JsonSubstatsDictionary[(int)Rarity];
    //         int i = 0;

    //         foreach (KeyValuePair<Attribute, float> substat in SubStats)
    //         {
    //             JsonSubstats[i] = new JsonSubstatsDictionary
    //             {
    //                 Key = substat.Key.ToString(),
    //                 Value = substat.Value
    //             };

    //             i++;
    //         }
    //     }

    //     WeaponSO = null;
    //     base.Serialize();
    // }

    // public override void Deserialize()
    // {
    //     base.Deserialize();

    //     if (Category == ItemCategory.Weapon || Rarity == global::Rarity.Common) return;
    //     SubStats = new();

    //     for (int i = 0; i < JsonSubstats.Length; i++)
    //     {
    //         SubStats[System.Enum.Parse<Attribute>(JsonSubstats[i].Key)] = JsonSubstats[i].Value;
    //         Debug.Log(JsonSubstats[i].Key);
    //     }
    // }

    protected override void SetName()
    {
        Name = $"[{Rarity}] {Category} {GetType().Name}";
    }
}