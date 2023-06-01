using PlayFab.EconomyModels;
using System;
using UnityEngine;

public class SummonTicket : Item
{
    public string Name;

    public SummonTicket(int rarity)
    {
        Rarity = (ItemRarity)rarity;
        Name = $"{Rarity} Summon Ticket";
        Stack = Rarity.ToString();
    }

    public SummonTicket(InventoryItem item)
    {
        SummonTicket summonTicket = JsonUtility.FromJson<SummonTicket>(item.DisplayProperties.ToString());

        Stack = item.StackId;
        Amount = (int)item.Amount;
        Rarity = summonTicket.Rarity;
        Name = summonTicket.Name;

        PlayFabManager.Instance.Inventory.SummonTickets.Add(this);
        Debug.Log($"Getting {Name} item !");
    }
}