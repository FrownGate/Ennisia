using UnityEngine;
using System.Collections.Generic;
using System;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Instance {  get; private set; }

    public static event Action<List<SupportCharacterSO>> OnSupportPulled;

    public SummonBannerSO SummonBanner;
    private readonly int _fragmentsPerDuplicate = 10;

    private Dictionary<int, int> _supports;
    private List<SupportCharacterSO> _pulledSupports;
    private int _amount;
    private double _chance;

    private readonly Dictionary<Rarity, SupportCharacterSO[]> _supportsPool = new();

    //TODO -> Add CheckCurrency script

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);

            foreach (var rarity in Enum.GetNames(typeof(Rarity)))
            {
                if (Enum.Parse<Rarity>(rarity) == Rarity.Common) continue;
                _supportsPool[Enum.Parse<Rarity>(rarity)] = Resources.LoadAll<SupportCharacterSO>($"SO/SupportsCharacter/{rarity}");
            }
        }
    }

    public bool CanPull(int amount)
    {
        _amount = amount;
        if (HasTicket()) return true;

        int cost = PlayFabManager.Instance.SummonCost * amount;
        if (!PlayFabManager.Instance.HasEnoughCurrency(cost, Currency.Crystals)) return false;

        return true;
    }

    private bool HasTicket()
    {
        Rarity[] raritiesToCheck = { Rarity.Legendary, Rarity.Epic };

        foreach (var rarity in raritiesToCheck)
        {
            Item ticket = PlayFabManager.Instance.Inventory.GetItem(new SummonTicket(), rarity);

            if (ticket != null && ticket.Amount >= _amount)
            {
                PlayFabManager.Instance.UseItem(ticket, _amount);
                _chance = PlayFabManager.Instance.Chances[rarity];
                return true;
            }
        }

        _chance = 100;
        return false;
    }

    public void Summon()
    {
        int newFragments = 0;

        _supports = PlayFabManager.Instance.GetSupports();
        _pulledSupports = new();

        for (int i = 0; i < _amount; i++)
        {
            SupportCharacterSO pulledSupport = GetSupport();
            _pulledSupports.Add(pulledSupport);
            Debug.Log($"{pulledSupport.Name} has been pulled !");

            if (_supports.ContainsKey(pulledSupport.Id))
            {
                if (_supports[pulledSupport.Id] < 5)
                {
                    _supports[pulledSupport.Id]++;
                    continue;
                }

                //newFragments += _fragmentsPerDuplicate * (int)pulledSupport.Rarity;
                continue;
            }

            _supports[pulledSupport.Id] = 1;
        }

        OnSupportPulled?.Invoke(_pulledSupports);
        PlayFabManager.Instance.AddSupports(_supports);
        //DisplayPull(); //TODO -> Use event

        if (newFragments == 0) return;
        PlayFabManager.Instance.AddCurrency(Currency.Fragments, newFragments);
        
    }

    private SupportCharacterSO GetSupport()
    {
        //TODO -> load only banner legendary characters if limited banner
        //TODO -> remove limited legendary characters if permanent banner
        SupportCharacterSO[] usedPool = _supportsPool[GetRarity()];
        int characterRoll = new System.Random().Next(1, usedPool.Length);
        Debug.Log($"character roll : {characterRoll}");
        return usedPool[characterRoll - 1];
    }

    private Rarity GetRarity()
    {
        Rarity rarity = Rarity.Rare;
        double rarityRoll = new System.Random().NextDouble() * _chance;
        Debug.Log($"rarity roll : {rarityRoll}");

        foreach (var chance in PlayFabManager.Instance.Chances)
        {
            if (rarityRoll <= chance.Value)
            {
                rarity = chance.Key;
                break;
            }
        }

        return rarity;
    }
}