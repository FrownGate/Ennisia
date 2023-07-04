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
    public void DropGear(Rarity rarity)
    {
        GearType type = GearType.Helmet;
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: type = GearType.Chest; break;
            case 1: type = GearType.Boots; break;
            case 2: type = GearType.Ring; break;
            case 3: type = GearType.Necklace; break;
            case 4: type = GearType.Earrings; break;
        }
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity,null));
    }
    public void DropGear(Dictionary<GearType, Rarity> gearList)
    {
        foreach(var gear in gearList)
        {
            PlayFabManager.Instance.AddInventoryItem(new Gear(gear.Key, gear.Value, null));
        }
    }
    public void DropCurrency(Dictionary<Currency, int> currencyList)
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
