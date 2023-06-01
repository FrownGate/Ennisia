using System;

[Serializable]
public class Item
{
    public enum ItemRarity
    {
        Common, Rare, Epic, Legendary
    }

    public ItemRarity Rarity;
}