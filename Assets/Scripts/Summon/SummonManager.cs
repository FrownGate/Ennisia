using UnityEngine;
using System.Collections.Generic;
using System;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Instance {  get; private set; }

    public static event Action<List<SupportCharacterSO>> OnSupportPulled;

    public SummonBannerSO SummonBanner;
    private int _amount;

    private readonly double _legendaryChance = 0.75;
    private readonly double _epicChance = 10.25;
    private readonly int _cost = 100;
    private readonly int _fragmentsPerDuplicate = 10;

    private Dictionary<int, int> _supports;
    private List<SupportCharacterSO> _pulledSupports;
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

    private double GetChance()
    {
        if (PlayFabManager.Instance.Inventory.GetItem(new SummonTicket(), Rarity.Legendary) != null) return _legendaryChance;
        if (PlayFabManager.Instance.Inventory.GetItem(new SummonTicket(), Rarity.Epic) != null) return _epicChance;
        return 100;
    }

    public void Summon()
    {
        _chance = GetChance();
        _amount = int.Parse(ScenesManager.Instance.Params);
        int newFragments = 0;

        //TODO -> Use tickets instead of crystals if _chance is < 100
        if (PlayFabManager.Instance.Currencies[Currency.Crystals] < _cost * _amount)
        {
            Debug.LogError("not enough crystals");
            //TODO -> Show UI error message
            return;
        }
        else
        {
            PlayFabManager.Instance.RemoveCurrency(Currency.Crystals, _cost * _amount);
        }

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

        if (rarityRoll <= _legendaryChance)
        {
            rarity = Rarity.Legendary;
        }
        else if (rarityRoll <= _epicChance)
        {
            rarity = Rarity.Epic;
        }

        return rarity;
    }
}