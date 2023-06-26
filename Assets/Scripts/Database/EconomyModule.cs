using PlayFab;
using PlayFab.EconomyModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayFabManager;

public class EconomyModule
{
    public Dictionary<Currency, int> Currencies { get; private set; } //Player's currencies
    public int Energy { get; private set; } //Player's energy

    private PlayFabManager _manager;

    private Dictionary<string, string> _currencies;
    private Dictionary<string, string> _itemsById;
    private Dictionary<string, string> _itemsByName;
    private struct CurrencyData { public int Initial; } //TODO -> move elsewhere ?

    public EconomyModule(PlayFabManager manager) { _manager = manager; }

    //Get catalog items and currencies

    public void GetEconomyData()
    {
        Debug.Log("Getting game currencies and catalog items...");
        PlayFabEconomyAPI.SearchItems(new(), OnGetDataSuccess, _manager.OnRequestError);
    }

    private void OnGetDataSuccess(SearchItemsResponse response)
    {
        _itemsById = new();
        _itemsByName = new();
        _currencies = new();
        Currencies = new();

        foreach (PlayFab.EconomyModels.CatalogItem item in response.Items)
        {
            //TODO -> Get bundle items and shops
            if (item.Type == "currency")
            {
                _currencies[item.Id] = item.AlternateIds[0].Value;
                CurrencyData data = JsonUtility.FromJson<CurrencyData>(item.DisplayProperties.ToString());
                Currencies[Enum.Parse<Currency>(_currencies[item.Id])] = data.Initial;
            }
            else if (item.Type == "catalogItem")
            {
                _itemsById[item.Id] = item.AlternateIds[0].Value;
                _itemsByName[item.AlternateIds[0].Value] = item.Id;
            }
        }

        //TODO -> check initial value of new currencies for existing players
        if (_firstLogin)
        {
            StartCoroutine(CreateInitialCurrencies());
        }
        else
        {
            GetEnergy();
        }
    }

    public void AddCurrency(Currency currency, int amount)
    {
        _manager.StartRequest($"Adding {amount} {currency}...");

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            //Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency.ToString()
                }
            }
        }, res => {
            Currencies[currency] += amount;
            _manager.InvokeOnCurrencyUpdate();
            _manager.InvokeOnCurrencyGained(currency);
            _manager.EndRequest($"Added {amount} {currency} !");
        }, _manager.OnRequestError);
    }

    public void RemoveCurrency(Currency currency, int amount)
    {
        _manager.StartRequest($"Removing {amount} {currency}...");

        PlayFabEconomyAPI.SubtractInventoryItems(new()
        {
            //Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency.ToString()
                }
            }
        }, res => {
            Currencies[currency] -= amount;
            _manager.InvokeOnCurrencyUpdate();
            _manager.InvokeOnCurrencyUsed(currency);
            _manager.EndRequest($"Removed {amount} {currency} !");
        }, _manager.OnRequestError);
    }

    public void AddEnergy(int amount)
    {
        _manager.StartRequest($"Adding {amount} energy...");

        PlayFabClientAPI.AddUserVirtualCurrency(new()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res => {
            Energy += amount;
            _manager.InvokeOnEnergyUpdate();
            _manager.EndRequest($"Added {amount} energy !");
        }, _manager.OnRequestError);
    }

    public void RemoveEnergy(int amount)
    {
        _manager.StartRequest($"Removing {amount} energy...");

        PlayFabClientAPI.SubtractUserVirtualCurrency(new()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res => {
            Energy -= amount;
            _manager.InvokeOnEnergyUpdate();
            _manager.InvokeOnEnergyUsed();
            _manager.EndRequest($"Removed {amount} energy !");
        }, _manager.OnRequestError);
    }

    public bool EnergyIsUsed(int amount)
    {
        if (amount >= Energy)
        {
            RemoveEnergy(amount);
        }
        else
        {
            Debug.LogError("Not enough energy");
        }

        return amount >= Energy;
    }
}