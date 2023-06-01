using System;

[Serializable]
public class Item
{
    public enum ItemRarity
    {
        Common, Rare, Epic, Legendary
    }

    [NonSerialized] public string Stack;
    [NonSerialized] public int Amount;
    public ItemRarity Rarity;

    //TODO -> Add Item to inventory or edit already existing one
}