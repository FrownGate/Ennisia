using PlayFab.EconomyModels;
using UnityEngine;

public class SummonTicket : Item
{
    public SummonTicket(int rarity, int amount = 1)
    {
        Rarity = (ItemRarity)rarity;
        Stack = Rarity.ToString();
        Amount = amount;

        AddToInventory();
    }

    public SummonTicket(InventoryItem item)
    {
        SummonTicket summonTicket = JsonUtility.FromJson<SummonTicket>(item.DisplayProperties.ToString());

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Rarity = summonTicket.Rarity;

        AddToInventory();
    }

    protected override void SetName()
    {
        Name = $"{Rarity} Summon Ticket";
    }
}