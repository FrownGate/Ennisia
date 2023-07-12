using PlayFab;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency
{
    Gold, Crystals, Fragments, EternalKeys, TerritoriesCurrency, Gemstone
}

public class EconomyModule : Module
{
    public static event Action OnInitComplete;
    public Dictionary<Currency, int> Currencies { get; private set; } //Player's currencies
    public int Energy { get; private set; } //Player's energy

    private Dictionary<string, string> _currencies;
    private Dictionary<string, string> _itemsById;
    private Dictionary<string, string> _itemsByName;
    private Dictionary<string, string> _storesById;
    private Dictionary<string, string> _storesByName;
    public Dictionary<string, CatalogItem> Stores;
    private Dictionary<string, string> _bundlesByName;
    private Dictionary<string, string> _bundlesById;

    private struct CurrencyData { public int Initial; } //TODO -> move elsewhere ?
    private bool _currencyAdded;

    //TODO -> Remove item from local inventory if database inventory isn't updated

    public void GetEconomyData()
    {
        _manager.StartRequest("Getting game currencies and catalog items...");
        PlayFabEconomyAPI.SearchItems(new(), OnGetDataSuccess, _manager.OnRequestError);
    }

    /// <summary>
    /// Get game currencies. New player -> Add initial currencies. Else -> Get currencies
    /// </summary>
    private void OnGetDataSuccess(SearchItemsResponse response)
    {
        _manager.EndRequest();
        _itemsById = new();
        _itemsByName = new();
        _currencies = new();
        _storesById = new();
        _storesByName = new();
        Stores = new();
        _bundlesById = new();
        _bundlesByName = new();
        Currencies = new();

        foreach (CatalogItem item in response.Items)
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
            else if (item.Type == "store")
            {
                _storesById[item.Id] = item.AlternateIds[0].Value;
                _storesByName[item.AlternateIds[0].Value] = item.Id;
                Stores[item.Id] = item;

            }
            else if (item.Type == "bundle")
            {
                _bundlesById[item.Id] = item.AlternateIds[0].Value;
                _bundlesByName[item.AlternateIds[0].Value] = item.Id;
            }
        }

        //TODO -> check initial value of new currencies for existing players
        if (_manager.IsFirstLogin)
        {
            StartCoroutine(CreateInitialCurrencies());
            return;
        }

        PlayFabClientAPI.GetUserInventory(new(), res =>
        {
            Energy = res.VirtualCurrency["EN"];
            //_manager.GetUserFiles();
            GetPlayerInventory();
        }, _manager.OnRequestError);
    }

    /// <summary>
    /// Create all initial currencies for new players
    /// </summary>
    private IEnumerator CreateInitialCurrencies()
    {
        foreach (KeyValuePair<Currency, int> currency in Currencies)
        {
            if (currency.Value == 0) continue;

            Debug.Log($"Giving initial {currency.Key}...");
            _currencyAdded = false;

            PlayFabEconomyAPI.AddInventoryItems(new()
            {
                Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
                Amount = currency.Value,
                Item = new()
                {
                    AlternateId = new()
                    {
                        Type = "FriendlyId",
                        Value = currency.Key.ToString()
                    }
                }
            }, res =>
            {
                Debug.Log($"{currency.Key} given !");
                _currencyAdded = true;
            }, _manager.OnRequestError);

            yield return new WaitUntil(() => _currencyAdded);
        }

        _manager.UpdateData();
        yield return null;
    }

    private void GetPlayerInventory()
    {
        _manager.StartRequest();

        PlayFabEconomyAPI.GetInventoryItems(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type }
        }, res =>
        {
            foreach (InventoryItem item in res.Items)
            {
                if (item.Type == "currency")
                {
                    Currencies[Enum.Parse<Currency>(_currencies[item.Id])] = (int)item.Amount;
                }
                else if (item.Type == "catalogItem")
                {
                    Type type = Type.GetType(_itemsById[item.Id]);
                    Activator.CreateInstance(type, item);
                }
            }

            _manager.Data.UpdateEquippedGears();
            _manager.EndRequest();
            OnInitComplete?.Invoke();
        }, _manager.OnRequestError);
    }

    public IEnumerator AddCurrency(Currency currency, int amount)
    {
        yield return _manager.StartAsyncRequest($"Adding {amount} {currency}...");

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency.ToString()
                }
            }
        }, res =>
        {
            Currencies[currency] += amount;
            _manager.InvokeOnCurrencyUpdate();
            _manager.InvokeOnCurrencyGained(currency);
            _manager.EndRequest($"Added {amount} {currency} !");
        }, _manager.OnRequestError);
    }

    public IEnumerator RemoveCurrency(Currency currency, int amount)
    {
        yield return _manager.StartAsyncRequest($"Removing {amount} {currency}...");

        PlayFabEconomyAPI.SubtractInventoryItems(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency.ToString()
                }
            }
        }, res =>
        {
            Currencies[currency] -= amount;
            _manager.InvokeOnCurrencyUpdate();
            _manager.InvokeOnCurrencyUsed(currency);
            _manager.EndRequest($"Removed {amount} {currency} !");
        }, _manager.OnRequestError);
    }

    public IEnumerator AddEnergy(int amount)
    {
        yield return _manager.StartAsyncRequest($"Adding {amount} energy...");

        PlayFabClientAPI.AddUserVirtualCurrency(new()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res =>
        {
            Energy += amount;
            _manager.InvokeOnEnergyUpdate();
            _manager.EndRequest($"Added {amount} energy !");
        }, _manager.OnRequestError);
    }

    public IEnumerator RemoveEnergy(int amount)
    {
        yield return _manager.StartAsyncRequest($"Removing {amount} energy...");

        PlayFabClientAPI.SubtractUserVirtualCurrency(new()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res =>
        {
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

    #region Items
    public IEnumerator AddInventoryItem(Item item)
    {
        yield return _manager.StartAsyncRequest("Adding item...");
        item.Serialize();

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = item.GetType().Name,
                },
                StackId = item.Stack ?? ""
            },
            NewStackValues = item != null ? new()
            {
                DisplayProperties = item
            } : new(),
            Amount = item.Amount
        }, res => _manager.EndRequest("Item added to inventory !"), _manager.OnRequestError);
    }

    public IEnumerator UpdateItem(Item item)
    {
        if (item == null || !_manager.Inventory.HasItem(item))
        {
            Debug.LogError("Item not found !");
            yield break;
        }

        yield return _manager.StartAsyncRequest();
        item.Serialize();

        PlayFabEconomyAPI.UpdateInventoryItems(new()
        {
            Item = new()
            {
                Id = _itemsByName[item.GetType().Name],
                Amount = item.Amount,
                DisplayProperties = item,
                StackId = item.Stack
            }
        }, res => _manager.EndRequest("Item updated !"), _manager.OnRequestError);
    }

    public IEnumerator UseItem(Item item, int amount = 1)
    {
        if (item == null || !_manager.Inventory.HasItem(item))
        {
            Debug.LogError("Item not found !");
            yield break;
        }

        yield return _manager.StartAsyncRequest();

        PlayFabEconomyAPI.SubtractInventoryItems(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = item.GetType().Name,
                },
                StackId = item.Stack
            },
            DeleteEmptyStacks = true,
            Amount = amount
        }, res =>
        {
            _manager.Inventory.RemoveItem(item);
            _manager.EndRequest("Item used !");
        }, _manager.OnRequestError);
    }

    public IEnumerator PurchaseInventoryItem(Item item)
    {
        yield return _manager.StartAsyncRequest($"Purchasing {item}...");

        PlayFabEconomyAPI.PurchaseInventoryItems(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = item.GetType().Name,
                },
                StackId = item.Stack
            },
            Amount = item.Amount
        }, res => _manager.EndRequest($"Purchased {item} !"), _manager.OnRequestError);
    }

    #endregion
}