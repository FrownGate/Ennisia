using System.Collections.Generic;
using UnityEngine;


public class RewardsDrop : MonoBehaviour
{
    public static RewardsDrop Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //random gear drop
    public void DropGear(Item.ItemRarity rarity)
    {
        Item.GearType type = Item.GearType.Helmet;
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: type = Item.GearType.Chest; break;
            case 1: type = Item.GearType.Boots; break;
            case 2: type = Item.GearType.Ring; break;
            case 3: type = Item.GearType.Necklace; break;
            case 4: type = Item.GearType.Earrings; break;
        }
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity));
    }
    public void DropGear(Dictionary<Item.GearType, Item.ItemRarity> gearList)
    {
        foreach(var gear in gearList)
        {
            PlayFabManager.Instance.AddInventoryItem(new Gear(gear.Key, gear.Value));
        }
    }
    public void DropCurrency(Dictionary<PlayFabManager.GameCurrency, int> currencyList)
    {
        foreach(var currency in currencyList)
        {
            PlayFabManager.Instance.AddCurrency(currency.Key,currency.Value);
        }
    }
    public void DropEnergy(int amount)
    {
        PlayFabManager.Instance.AddEnergy(amount);
    }

}
