using System.Collections.Generic;
using UnityEngine;
using System;


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
        int count = Enum.GetNames(typeof(GearType)).Length;
        GearType type = (GearType)UnityEngine.Random.Range(0, count);
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
