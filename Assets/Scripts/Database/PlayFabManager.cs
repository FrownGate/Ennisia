using PlayFab;
using PlayFab.ClientModels;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using PlayFab.DataModels;
using PlayFab.EconomyModels;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance { get; private set; }
    public static event Action OnLoginSuccess;
    public static event Action<PlayFabError> OnError;
    public static event Action OnCurrencyUpdate;
    public static event Action OnEnergyUpdate;

    public AccountData Account { get; private set; }
    public PlayerData Player { get; private set; }
    public InventoryData Inventory { get; private set; }
    public Dictionary<string, int> Currencies { get; private set; }
    public int Energy { get; private set; }
    public string PlayFabId { get; private set; }
    public PlayFab.ClientModels.EntityKey Entity { get; private set; }
    public bool LoggedIn { get; private set; }

    private Dictionary<string, string> _currencies;
    private Dictionary<string, string> _itemsById;
    private Dictionary<string, string> _itemsByName;

    private struct CurrencyData
    {
        public int Initial;
    }

    private Dictionary<string, Data> _datas;
    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path;
    private bool _firstLogin;
    private bool _currencyAdded;

    //TODO -> update items
    //TODO -> use Player Datas for Supports instead ?

    #region 1 - Login
    //HasLocalSave -> Login
    //Else -> Anonymous Login

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

            _authData = new();
            _binaryFormatter = new();
            _path = Application.persistentDataPath + "/ennisia.save";
            Debug.Log($"Your save path is : {_path}");
            LoggedIn = false;

            //TODO : Parfois la save est faussement found
            if (HasLocalSave()) return;
            Debug.Log("No local save found -> anonymous login");
            AnonymousLogin();

            //Use this line instead of AnonymousLogin to test PlayFab Login with no local save
            //Login("testing@gmail.com", "Testing");
        }
    }

    private bool HasLocalSave()
    {
        //Check if binary file with user datas exists
        if (!File.Exists(_path)) return false;

        try
        {
            using (FileStream file = new(_path, FileMode.Open))
            {
                _authData = (AuthData)_binaryFormatter.Deserialize(file);
            }

            Debug.Log("Loading local save...");
            Login(_authData.Email, _authData.Password);
            return true;
        }
        catch
        {
            File.Delete(_path);
            return false;
        }
    }

    private bool HasAuthData()
    {
        return !string.IsNullOrEmpty(_authData.Email) && !string.IsNullOrEmpty(_authData.Email);
    }

    private void CreateAccountData(string email, string password)
    {
        //Create binary file with user datas
        _authData = new()
        {
            Email = email,
            Password = password
        };
    }

    private void Login(string email, string password)
    {
        if (_authData == null) CreateAccountData(email, password);

        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest()
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true
            }
        }, OnLoginRequestSuccess, OnLoginRequestError);
    }

    private void AnonymousLogin()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true
            }
        }, OnLoginRequestSuccess, OnRequestError);
    }

    private void OnLoginRequestSuccess(LoginResult result)
    {
        Debug.Log("Login request success !");
        CreateLocalData(CreateUsername());

        PlayFabId = result.PlayFabId;
        Entity = result.EntityToken.Entity;

        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        _firstLogin = info.Created == info.TitleInfo.Created && result.LastLoginTime == null;

        if (HasAuthData()) CreateSave();

        GetEconomyData();

        //Use this line once to test PlayFab Register & Login
        //RegisterAccount("testing@gmail.com", "Testing");
    }

    private void CreateLocalData(string username)
    {
        Account = new AccountData(username);
        Player = new PlayerData();
        Inventory = new InventoryData();

        _datas = new()
        {
            [Account.GetName()] = Account,
            [Player.GetName()] = Player,
            [Inventory.GetName()] = Inventory
        };
    }

    private void OnLoginRequestError(PlayFabError error)
    {
        OnRequestError(error);
        _authData ??= null;

        if (File.Exists(_path)) File.Delete(_path);
    }
    #endregion

    #region 2 - Game Datas
    //Get catalog items and currencies

    private void GetEconomyData()
    {
        Debug.Log("Getting game currencies and catalog items...");
        PlayFabEconomyAPI.SearchItems(new(), OnGetDataSuccess, OnRequestError);
    }

    private void OnGetDataSuccess(SearchItemsResponse response)
    {
        _itemsById = new();
        _itemsByName = new();
        _currencies = new();
        Currencies = new();

        foreach (PlayFab.EconomyModels.CatalogItem item in response.Items)
        {
            if (item.Type == "currency")
            {
                _currencies[item.Id] = item.AlternateIds[0].Value;
                CurrencyData data = JsonUtility.FromJson<CurrencyData>(item.DisplayProperties.ToString());
                Currencies[_currencies[item.Id]] = data.Initial;
            }
            else if (item.Type == "catalogItem")
            {
                _itemsById[item.Id] = item.AlternateIds[0].Value;
                _itemsByName[item.AlternateIds[0].Value] = item.Id;
            }
        }

        if (_firstLogin)
        {
            StartCoroutine(CreateInitialCurrencies());
        }
        else
        {
            GetPlayerInventory();
        }
    }

    public void GetPlayerInventory()
    {
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type }
        }, OnGetPlayerInventorySuccess, OnRequestError);
    }

    private void OnGetPlayerInventorySuccess(GetInventoryItemsResponse response)
    {
        foreach (InventoryItem item in response.Items)
        {
            if (item.Type == "currency")
            {
                Currencies[_currencies[item.Id]] = (int)item.Amount;
            }
            else if (item.Type == "catalogItem")
            {
                Debug.Log("Creating item instance");
                Type type = Type.GetType(_itemsById[item.Id]);
                Activator.CreateInstance(type, item);
            }
        }

        GetEnergy();
    }
    #endregion

    #region 3 - Currencies
    //Get game currencies
    //New player -> Add initial currencies
    //Else -> Get currencies

    private IEnumerator CreateInitialCurrencies()
    {
        foreach(KeyValuePair<string, int> currency in Currencies)
        {
            if (currency.Value == 0) continue;

            Debug.Log($"Giving initial {currency.Key}...");
            _currencyAdded = false;

            PlayFabEconomyAPI.AddInventoryItems(new()
            {
                Entity = new() { Id = Entity.Id, Type = Entity.Type },
                Amount = currency.Value,
                Item = new()
                {
                    AlternateId = new()
                    {
                        Type = "FriendlyId",
                        Value = currency.Key
                    }
                }
            }, res => {
                Debug.Log($"{currency.Key} given !");
                _currencyAdded = true;
            }, OnRequestError);

            yield return new WaitUntil(() => _currencyAdded);
        }

        UpdateData();
        yield return null;
    }

    public void GetEnergy()
    {
        PlayFabClientAPI.GetUserInventory(new() { }, OnGetEnergySuccess, OnRequestError);
    }

    private void OnGetEnergySuccess(GetUserInventoryResult result)
    {
        Energy = result.VirtualCurrency["EN"];
        GetUserDatas();
    }
    #endregion

    #region 4 - User Datas
    //Datas = Account, Player, Inventory
    //New player -> Create datas then Update
    //Else -> Get existing datas
    //Set LoggedIn as true and invoke Login event

    private void UpdateData()
    {
        List<SetObject> objects = new();

        foreach (KeyValuePair<string, Data> data in _datas)
        {
            objects.Add(data.Value.Serialize());
        }

        PlayFabDataAPI.SetObjects(new SetObjectsRequest
        {
            Objects = objects,
            Entity = new()
            {
                Id = Entity.Id,
                Type = Entity.Type
            }
        }, res => {
            Debug.Log("Data updated !");
            CompleteLogin();
        }, OnRequestError);
    }

    private void GetUserDatas()
    {
        PlayFabDataAPI.GetObjects(new GetObjectsRequest
        {
            EscapeObject = true,
            Entity = new()
            {
                Id = Entity.Id,
                Type = Entity.Type
            }
        }, OnDataObtained, OnRequestError);
    }

    private void OnDataObtained(GetObjectsResponse response)
    {
        bool DataIsUpdated = true;

        foreach (KeyValuePair<string, Data> data in _datas)
        {
            if (!response.Objects.ContainsKey(data.Key))
            {
                DataIsUpdated = false;
                continue;
            }

            _datas[data.Key].UpdateLocalData(response.Objects[data.Key].EscapedDataObject);
        }

        if (!DataIsUpdated)
        {
            Debug.LogWarning("data missing - creating missing ones...");
            UpdateData();
            return;
        }

        CompleteLogin();
    }

    private void CompleteLogin()
    {
        if (!LoggedIn)
        {
            if (_firstLogin) UpdateName(Account.Name);
            _firstLogin = false;
            LoggedIn = true;
            OnLoginSuccess?.Invoke();
            Debug.Log("Login complete !");
            Testing();
        }
    }
    #endregion

    #region Economy
    public void AddCurrency(string currency, int amount)
    {
        Debug.Log($"Adding {amount} {currency}...");

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency
                }
            }
        }, res => {
            Debug.Log($"Added {amount} {currency} !");
            Currencies[currency] += amount;
            OnCurrencyUpdate?.Invoke();
        }, OnRequestError);
    }

    public void RemoveCurrency(string currency, int amount)
    {
        Debug.Log($"Removing {amount} {currency}...");
        PlayFabEconomyAPI.SubtractInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency
                }
            }
        }, res => {
            Debug.Log($"Removed {amount} {currency} !");
            Currencies[currency] -= amount;
            OnCurrencyUpdate?.Invoke();
        }, OnRequestError);
    }

    public void AddEnergy(int amount)
    {
        Debug.Log($"Adding {amount} energy...");
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res => {
            Debug.Log($"Added {amount} energy !");
            Energy += amount;
            OnEnergyUpdate?.Invoke();
        }, OnRequestError);
    }

    public void RemoveEnergy(int amount)
    {
        Debug.Log($"Removing {amount} energy...");
        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res => {
            Debug.Log($"Removed {amount} energy !");
            Energy -= amount;
            OnEnergyUpdate?.Invoke();
        }, OnRequestError);
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
    #endregion

    #region Gacha
    public Dictionary<int, int> GetSupports()
    {
        Dictionary<int, int> supports = new();

        foreach (SupportData support in Inventory.Supports)
        {
            supports[support.Id] = support.Lvl;
        }

        return supports;
    }

    public int HasSupport(int id)
    {
        for (int i = 0; i < Inventory.Supports.Count; i++)
        {
            if (Inventory.Supports[i].Id == id) return i;
        }

        return 0;
    }

    public void AddSupports(Dictionary<int, int> pulledSupports)
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

        Inventory.Supports = supports;
        UpdateData();
    }
    #endregion

    #region Equipment
    public void SetGearData(EquipmentSO equipment, int id)
    {
        //TODO -> Use find
        //foreach (Gear inventoryGear in Inventory.GetGears())
        //{
        //    if (inventoryGear.Id == id)
        //    {
        //        equipment.Id = inventoryGear.Id;
        //        equipment.Name = inventoryGear.Name;
        //        equipment.Type = (Item.GearType)inventoryGear.Type;
        //        equipment.Rarity = (Item.ItemRarity)inventoryGear.Rarity; //TODO -> Update Equipment SO
        //        equipment.Attribute = (Item.AttributeStat)inventoryGear.Attribute;
        //        equipment.StatValue = inventoryGear.Value;
        //        equipment.Description = inventoryGear.Description;
        //        break;
        //    }
        //}
    }
    #endregion

    #region Items
    public void AddInventoryItem(Item item)
    {
        item.Serialize();

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Item = new InventoryItemReference
            {
                AlternateId = new AlternateId
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
        }, res =>
        {
            Debug.Log("inventory added");
        }, OnRequestError);
    }
    #endregion

    #region Account
    private void RegisterAccount(string email, string password) //This function will be registered to a button event
    {
        Debug.Log("Register account...");
        CreateAccountData(email, password);
        string username = CreateUsername(email);

        PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest()
        {
            Username = username, //Create unique username with email
            Email = email,
            Password = password //Password must be between 6 and 100 characters
        },
        res => {
            UpdateName(username);
            CreateSave();
        }, OnRequestError);
    }

    private void UpdateName(string name)
    {
        Debug.Log("Updating username...");
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        }, null, OnRequestError);
    }
    #endregion

    #region General
    private string CreateUsername(string email = "user")
    {
        string name = email.Split('@')[0];
        name += SystemInfo.deviceUniqueIdentifier[..5];
        Debug.Log($"Creating username {name}...");
        return name;
    }

    private void CreateSave()
    {
        Debug.Log("Creating local save...");

        using (FileStream file = new(_path, FileMode.OpenOrCreate))
        {
            _binaryFormatter.Serialize(file, _authData);
        }

        _authData = new();
    }

    public void OnRequestError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        OnError?.Invoke(error);
    }
    #endregion

    //Called after login success to test code
    private void Testing()
    {
        //Debug.Log("Testing");
        //AddInventoryItem(new Gear(Item.GearType.Boots, Item.ItemRarity.Rare));
        //AddInventoryItem(new Material(Item.ItemCategory.Weapon, Item.ItemRarity.Legendary, 5));
        //AddInventoryItem(new SummonTicket(Item.ItemRarity.Common));
    }
}