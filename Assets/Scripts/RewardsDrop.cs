using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

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
                foreach (var matReward in missionSO.MaterialsRewards)
                {
                    DropMaterial(missionSO.MaterialsRewards, _matAmount[matReward.Key]);
                    DropCurrency(missionSO.CurrencyRewards);
                }
                break;
            case MissionManager.MissionType.Raid:
                for (int i = 0; i < missionSO.GearReward.Count; i++)
                {
                    DropGear(missionSO.GearReward[i]);
                    DropCurrency(missionSO.CurrencyRewards);

                }
                break;
            case MissionManager.MissionType.EndlessTower:
                foreach (var matReward in missionSO.MaterialsRewards)
                {
                    DropMaterial(missionSO.MaterialsRewards, _matAmount[matReward.Key]);
                }
                foreach (var ticket in missionSO.Tickets)
                {
                    DropTicket(ticket);
                }
                DropCurrency(missionSO.CurrencyRewards);

                break;
        }
        ExperienceSystem.Instance.GainExperienceAccount(missionSO.Experience);
        ExperienceSystem.Instance.GainExperiencePlayer(missionSO.Experience);
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
            Debug.Log("gear drop : " + gear.Key + " " + gear.Value);
            PlayFabManager.Instance.AddInventoryItem(new Gear(gear.Key, gear.Value, null));
        }
    }
    private void DropCurrency(Dictionary<Currency, int> currencyList)
    {
        foreach (var currency in currencyList)
        {
            Debug.Log(currency.Value + " " + currency.Key + " drop");
            PlayFabManager.Instance.AddCurrency(currency.Key, currency.Value);
        }
    }

    private void DropMaterial(Dictionary<ItemCategory, Rarity> materialsRewards, int materialAmount)
    {
        foreach (var reward in materialsRewards)
        {
            Material material = new Material(reward.Key, reward.Value, materialAmount);
            Debug.Log("material drop : " + material.Category + " x" + material.Amount);
            PlayFabManager.Instance.AddInventoryItem(material);
        }
    }

    private void DropTicket(KeyValuePair<Rarity, int> ticket)
    {
        SummonTicket summonTicket = new SummonTicket(ticket.Key, ticket.Value);
        Debug.Log("ticket drop : " + summonTicket.Rarity + " x" + summonTicket.Amount);
        PlayFabManager.Instance.AddInventoryItem(summonTicket);

    }

    private void DropEnergy(int amount)
    {
        Debug.Log(amount + " energy drop");
        PlayFabManager.Instance.AddEnergy(amount);
    }


    private void OnEnable()
    {
        MissionManager.OnMissionComplete += Drop;
    }
    private void OnDisable()
    {
        MissionManager.OnMissionComplete -= Drop;
    }
}
