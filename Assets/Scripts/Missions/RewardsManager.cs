using System.Collections.Generic;
using UnityEngine;
using System;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance { get; private set; }
    public static event Action<int> GainXp;

    private Dictionary<ItemCategory, int> _matAmount;
    public List<Item> Rewards;
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

        foreach (RewardData reward in missionSO.RewardData)
        {
            Type type = Type.GetType(CSVUtils.GetFileName(reward.Name));            
            Rewards.Add((Item)Activator.CreateInstance(type));
        }
        

        GainXp?.Invoke(missionSO.Experience);
    }

    private void Drop(Dictionary<Currency, int> currencyList)
    {
        foreach (var currency in currencyList)
        {
            Debug.Log(currency.Value + " " + currency.Key + " drop");
            PlayFabManager.Instance.AddCurrency(currency.Key, currency.Value);
        }
    }
 
    private void Drop(int energy)
    {
        Debug.Log(energy + " energy drop");
        PlayFabManager.Instance.AddEnergy(energy);
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