using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class GearData
{
    public int Id;
    public string Name;
    public string Type;
    public string Rarity;
    public string Attribute;
    public float Value;
    public string Description;
    public string Icon;

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