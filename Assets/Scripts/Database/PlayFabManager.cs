using PlayFab;
using PlayFab.ClientModels;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using PlayFab.DataModels;
using PlayFab.EconomyModels;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance { get; private set; }
    public static event Action OnLoginSuccess;
    public static event Action<PlayFabError> OnError;
    public static event Action<List<InventoryItem>> OnGetCurrencies;
    public static event Action<int> OnGetEnergy;
    public static event Action OnCurrencyUpdate;

    public AccountData Account { get; private set; }
    public PlayerData Player { get; private set; }
    public InventoryData Inventory { get; private set; }
    public Dictionary<string, string> GameCurrencies { get; private set; }
    public Dictionary<string, int> Currencies { get; private set; }
    public int Energy { get; private set; }
    public string PlayFabId { get; private set; }
    public PlayFab.ClientModels.EntityKey Entity { get; private set; }
    public bool LoggedIn { get; private set; }

    private struct CurrencyData
    {
        public int Initial;
    }

    private Data[] _datas;
    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path;
    private bool _newPlayer;

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

            _binaryFormatter = new();
            _path = Application.persistentDataPath + "/ennisia.save";
            Debug.Log($"Your save path is : {_path}");
            LoggedIn = false;

            //TODO : Parfois la save est faussement found
            if (HasLocalSave()) return;
            Debug.Log("no save found");
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
        }
        catch
        {
            File.Delete(_path);
            return false;
        }

        Debug.Log("save found");
        Login(_authData.email, _authData.password);
        return true;
    }

    private void CreateAccountData(string email, string password)
    {
        //Create binary file with user datas
        _authData = new()
        {
            email = email,
            password = password
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
        Debug.Log("anonymous login");
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
        _datas = new Data[3]
        {
            Account = new AccountData(CreateUsername()),
            Player = new PlayerData(),
            Inventory = new InventoryData()
        };

        Debug.Log("login success");
        PlayFabId = result.PlayFabId;
        Entity = result.EntityToken.Entity;

        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        _newPlayer = info.Created == info.TitleInfo.Created && result.LastLoginTime == null;

        if (_authData != null) CreateSave();

        GetCurrencies();

        //Use this line once to test PlayFab Register & Login
        //RegisterAccount("testing@gmail.com", "Testing");
        Debug.Log("PlayFabId: " + result.PlayFabId);
        Debug.Log("SessionTicket: " + result.SessionTicket);
        Debug.Log("EntityToken: " + result.EntityToken.EntityToken);
    }

    private void OnLoginRequestError(PlayFabError error)
    {
        Debug.Log("success");

        OnRequestError(error);
        _authData ??= null;

        if (File.Exists(_path)) File.Delete(_path);
    }
    #endregion

    #region 2 - Currencies
    //Get game currencies
    //New player -> Add initial currencies
    //Else -> Get currencies

    private void GetCurrencies()
    {
        Debug.Log("Getting game currencies...");
        PlayFabEconomyAPI.SearchItems(new()
        {
            Filter = $"ContentType eq 'currency'"
        }, OnGetCurrencySuccess, OnRequestError);
    }

    private void OnGetCurrencySuccess(SearchItemsResponse response)
    {
        GameCurrencies = new();
        Currencies = new();

        foreach (PlayFab.EconomyModels.CatalogItem item in response.Items)
        {
            GameCurrencies[item.Id] = item.AlternateIds[0].Value;
            CurrencyData data = JsonUtility.FromJson<CurrencyData>(item.DisplayProperties.ToString());
            Currencies[GameCurrencies[item.Id]] = data.Initial;
        }

        if (_newPlayer)
        {
            CreateInitialCurrencies();
        }
        else
        {
            GetPlayerCurrencies();
        }
    }

    private void CreateInitialCurrencies()
    {
        foreach(KeyValuePair<string, int> currency in Currencies)
        {
            if (currency.Value == 0) continue;

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
            }, null, OnRequestError);
        }

        UpdateData();
    }

    public void GetPlayerCurrencies()
    {
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Filter = $"stackId eq 'currency'"
        }, OnGetPlayerCurrenciesSuccess, OnRequestError);
    }

    private void OnGetPlayerCurrenciesSuccess(GetInventoryItemsResponse response)
    {
        foreach (InventoryItem item in response.Items)
        {
            Currencies[GameCurrencies[item.Id]] = (int)item.Amount;
        }

        OnGetCurrencies?.Invoke(response.Items); //temp
        GetEnergy();
    }

    public void GetEnergy()
    {
        PlayFabClientAPI.GetUserInventory(new() { }, OnGetEnergySuccess, OnRequestError);
    }

    private void OnGetEnergySuccess(GetUserInventoryResult result)
    {
        Energy = result.VirtualCurrency["EN"];
        OnGetEnergy?.Invoke(Energy); //temp ?
        GetUserDatas();
    }
    #endregion

    #region 3 - User Datas
    //Datas = Account, Player, Inventory
    //New player -> Create datas then Update
    //Else -> Get existing datas
    //Set LoggedIn as true and invoke Login event

    private void UpdateData()
    {
        List<SetObject> data = new();

        for (int i = 0; i < _datas.Length; i++)
        {
            data.Add(_datas[i].Serialize());
        }

        PlayFabDataAPI.SetObjects(new SetObjectsRequest
        {
            Objects = data,
            Entity = new()
            {
                Id = Entity.Id,
                Type = Entity.Type
            }
        }, res => { Login(); }, OnRequestError);
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
        try
        {
            for (int i = 0; i < _datas.Length; i++)
            {
                _datas[i].UpdateLocalData(response.Objects[_datas[i].ClassName].EscapedDataObject);
            }

            Login();
            Debug.Log("data obtained");

        }
        catch
        {
            Debug.LogWarning("data missing - creating missing ones...");
            UpdateData();
        }
    }

    private void Login()
    {
        if (!LoggedIn)
        {
            LoggedIn = true;
            OnLoginSuccess?.Invoke();
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
            Debug.Log("Done !");
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
            Debug.Log("Done !");
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
            Debug.Log("Done !");
            Energy += amount;
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
            Debug.Log("Done !");
            Energy -= amount;
        }, OnRequestError);
    }
    #endregion

    #region Gacha
    public Dictionary<int, int> GetSupports()
    {
        Dictionary<int, int> supports = new();

        foreach (SupportData support in Inventory.Supports)
        {
            supports[support.Id] = support.Level;
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
                Level = support.Value
            });
        }

        Inventory.Supports = supports;
        UpdateData();
    }
    #endregion

    #region Account
    private void RegisterAccount(string email, string password) //This function will be registered to a button event
    {
        CreateAccountData(email, password);

        PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest()
        {
            Username = CreateUsername(email), //Create unique username with email
            Email = email,
            Password = password //Password must be between 6 and 100 characters
        },
        res => { CreateSave(); }, OnRequestError);
    }

    private void UpdateName(string name)
    {
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
        Debug.Log($"creating username {name}");
        UpdateName(name);
        return name;
    }

    private void CreateSave()
    {
        using (FileStream file = new(_path, FileMode.OpenOrCreate))
        {
            _binaryFormatter.Serialize(file, _authData);
        }

        _authData = null;
    }

    public void OnRequestError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        OnError?.Invoke(error);
    }
    #endregion
}