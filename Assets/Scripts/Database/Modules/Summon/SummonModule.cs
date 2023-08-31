using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonModule : Module
{
    [SerializeField] private double _legendaryChance = 0.75;
    [SerializeField] private double _epicChance = 10.25;

    public readonly int SummonCost = 100;
    public readonly int FragmentsGain = 10;
    public Dictionary<Rarity, double> Chances;

    private int _amount;
    private double _chance;
    private Dictionary<int, int> _alreadyPulledSupports;
    private List<SupportCharacterSO> _pulledSupports;
    private readonly Dictionary<Rarity, SupportCharacterSO[]> _supportsPool = new();

    private void Awake()
    {
        Chances = new()
        {
            [Rarity.Legendary] = _legendaryChance,
            [Rarity.Epic] = _epicChance
        };

        foreach (var rarity in Enum.GetNames(typeof(Rarity)))
        {
            if (Enum.Parse<Rarity>(rarity) == Rarity.Common) continue;
            _supportsPool[Enum.Parse<Rarity>(rarity)] = Resources.LoadAll<SupportCharacterSO>($"SO/SupportsCharacter/{rarity}");
        }
    }

    private Dictionary<int, int> GetSupports()
    {
        Dictionary<int, int> supports = new();

        foreach (SupportData support in _manager.Inventory.Supports)
        {
            supports[support.Id] = support.Lvl;
        }

        return supports;
    }

    private void AddSupports(Dictionary<int, int> pulledSupports)
    {
        List<SupportData> supports = new();

        foreach (KeyValuePair<int, int> support in pulledSupports)
        {
            supports.Add(new()
            {
                Id = support.Key,
                Lvl = support.Value
            });
        }

        _manager.Inventory.Supports = supports;
        _manager.UpdateData();
    }

    public void Summon()
    {
        int newFragments = 0;

        _alreadyPulledSupports = GetSupports();
        _pulledSupports = new();

        for (int i = 0; i < _amount; i++)
        {
            SupportCharacterSO pulledSupport = GetSupport();
            _pulledSupports.Add(pulledSupport);
            Debug.Log($"{pulledSupport.Name} has been pulled !");

            if (_alreadyPulledSupports.ContainsKey(pulledSupport.Id))
            {
                if (_alreadyPulledSupports[pulledSupport.Id] < 5)
                {
                    _alreadyPulledSupports[pulledSupport.Id]++;
                    continue;
                }

                newFragments += FragmentsGain * (int)pulledSupport.Rarity;
                continue;
            }

            _alreadyPulledSupports[pulledSupport.Id] = 1;
        }

        AddSupports(_alreadyPulledSupports);
        _manager.InvokeOnSummon(_pulledSupports);

        if (newFragments == 0) return;
        _manager.AddCurrency(Currency.Fragments, newFragments);

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

        foreach (var chance in Chances)
        {
            if (rarityRoll <= chance.Value)
            {
                rarity = chance.Key;
                break;
            }
        }

        return rarity;
    }

    public bool CanPull(int amount)
    {
        _amount = amount;
        if (HasTicket()) return true;

        int cost = SummonCost * amount;
        if (!_manager.HasEnoughCurrency(cost, Currency.Crystals)) return false;

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
                _manager.UseItem(ticket, _amount);
                _chance = Chances[rarity];
                return true;
            }
        }

        _chance = 100;
        return false;
    }
}