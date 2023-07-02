using PlayFab.EconomyModels;
using UnityEngine;

public class SummonTicket : Item
{
    public SummonTicket() { }

    public SummonTicket(Rarity rarity, int amount = 1)
    {
        Rarity = rarity;
        Stack = Rarity.ToString();
        Amount = amount;

        AddToInventory();
    }

    public SummonTicket(InventoryItem item)
    {
        SummonTicket summonTicket = JsonUtility.FromJson<SummonTicket>(item.DisplayProperties.ToString());
        summonTicket.Deserialize();

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Rarity = summonTicket.Rarity;
        Name = summonTicket.Name;

        AddToInventory();
    }

    protected override void SetName()
    {
        Name = Rarity == global::Rarity.Common ? "Summon Ticket" : $"{Rarity} Summon Ticket";
    }
}