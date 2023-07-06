using System.Collections.Generic;
using UnityEngine;
using System;


public class RewardsDrop : MonoBehaviour
{
    public static RewardsDrop Instance { get; private set; }
    private Dictionary<ItemCategory, int> _matAmount;
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

    public void Drop(MissionSO missionSO)
    {
        _matAmount = missionSO.MatAmount;
        switch (missionSO.Type)
        {
            case MissionManager.MissionType.Dungeon:
                DropMaterial(missionSO.MaterialsRewards, _matAmount[missionSO.MaterialsRewards.Key]);
                break;
            case MissionManager.MissionType.Raid:
                for (int i = 0; i < missionSO.GearReward.Count; i++)
                {
                    DropGear(missionSO.GearReward[i]);
                }
                break;
            case MissionManager.MissionType.EndlessTower:

                DropMaterial(missionSO.MaterialsRewards, _matAmount[missionSO.MaterialsRewards.Key]);
                break;


        }
    }

    //random gear drop
    private void DropGear(Rarity rarity)
    {
        int count = Enum.GetNames(typeof(GearType)).Length;
        GearType type = (GearType)UnityEngine.Random.Range(0, count);
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity, null));
    }
    private void DropGear(Dictionary<GearType, Rarity> gearList)
    {
        foreach (var gear in gearList)
        {
            PlayFabManager.Instance.AddInventoryItem(new Gear(gear.Key, gear.Value, null));
        }
    }
    private void DropCurrency(Dictionary<Currency, int> currencyList)
    {
        foreach (var currency in currencyList)
        {
            PlayFabManager.Instance.AddCurrency(currency.Key, currency.Value);
        }
    }

    private void DropMaterial(Dictionary<ItemCategory, Rarity> materialsRewards, int materialAmount)
    {
        foreach (var reward in materialsRewards)
        {
            Material material = new Material(reward.Key, reward.Value, materialAmount);
            PlayFabManager.Instance.AddInventoryItem(material);
        }
    }
    private void DropEnergy(int amount)
    {
        PlayFabManager.Instance.AddEnergy(amount);
    }

}
