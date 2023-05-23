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

    public struct Account
    {
        public string Name;
        public int Level;
        public int Exp;
        public int Gender;
        public bool Tutorial;
    }

    public struct Player
    {
        public int Level;
        public int Exp;
        public int EquippedWeapon;
        public int[] EquippedGears;
        public int[] EquippedSupports;
    }

    public struct Currencies
    {
        //
    }

    public Account AccountData { get; private set; }
    public Player PlayerData { get; private set; }
    public string PlayFabId { get; private set; }
    public PlayFab.ClientModels.EntityKey Entity { get; private set; }

    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path;

    private readonly string _goldId = "0dc3228a-78a9-4d26-a3ab-f7d1e5b5c4d3";
    private readonly string _crystalsId = "0ec7fd19-4c26-4e0a-bd66-cf94f83ef060";

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

            if (HasSave()) return;
            Debug.Log("no save found");
            AnonymousLogin();

            //Use this line instead of AnonymousLogin to test PlayFab Login with no local save
            //Login("testing@gmail.com", "Testing");
        }
    }

    private bool HasSave()
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
        Debug.Log("login success");
        OnLoginSuccess?.Invoke();
        PlayFabId = result.PlayFabId;
        Entity = result.EntityToken.Entity;

        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        bool newPlayer = info.Created == info.TitleInfo.Created && result.LastLoginTime == null;

        if (newPlayer)
        {
            Debug.Log("new player");
            CreateData();
        }
        else
        {
            GetUserDatas();
        }

        if (_authData != null) CreateSave();

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

    public void OnRequestError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        OnError?.Invoke(error);
    }

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

    private void CreateAccountData(string email, string password)
    {
        //Create binary file with user datas
        _authData = new()
        {
            email = email,
            password = password
        };
    }

    private void CreateSave()
    {
        using (FileStream file = new(_path, FileMode.OpenOrCreate))
        {
            _binaryFormatter.Serialize(file, _authData);
        }

        _authData = null;
    }

    private string CreateUsername(string email = "user")
    {
        string name = email.Split('@')[0];
        name += SystemInfo.deviceUniqueIdentifier[..5];
        Debug.Log($"creating username {name}");
        UpdateName(name);
        return name;
    }

    private void CreateData()
    {
        AccountData = new()
        {
            Name = CreateUsername(),
            Level = 1
        };

        PlayerData = new()
        {
            Level = 1,
            EquippedGears = new int[6],
            EquippedSupports = new int[2]
        };

        UpdateData();
        AddCurrency("Gold", 1000);
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

    private void UpdateData()
    {
        PlayFabDataAPI.SetObjects(new SetObjectsRequest
        {
            Objects = new()
            {
                new SetObject
                {
                    ObjectName = "Account",
                    EscapedDataObject = JsonUtility.ToJson(AccountData)
                },
                new SetObject
                {
                    ObjectName = "Player",
                    EscapedDataObject = JsonUtility.ToJson(PlayerData)
                }
            },
            Entity = new()
            {
                Id = Entity.Id,
                Type = Entity.Type
            }
        }, res => { Debug.Log("data updated"); }, OnRequestError);
    }

    private void OnDataObtained(GetObjectsResponse response)
    {
        AccountData = JsonUtility.FromJson<Account>(response.Objects["Account"].EscapedDataObject);
        PlayerData = JsonUtility.FromJson<Player>(response.Objects["Player"].EscapedDataObject);
        Debug.Log("data obtained");
    }

    private void UpdateName(string name)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        }, null, OnRequestError);
    }

    public void AddCurrency(string currency, int amount)
    {
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
        }, OnCurrencyAdd, OnRequestError);
    }

    public void RemoveCurrency(string currency, int amount)
    {
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
        }, OnCurrencySubtract, OnRequestError);
    }

    private void OnCurrencyAdd(AddInventoryItemsResponse response)
    {
        //Update user inventory
    }

    private void OnCurrencySubtract(SubtractInventoryItemsResponse response)
    {
        //Update user inventory
    }
    public void GetCurrency()
    {
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Filter = $"stackId eq 'currency'"
        }, OnGetCurrencySuccess, OnGetCurrencyError);

    }

    public void GetCurrency(string currency)
    {
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Filter = $":{currency}"
        }, OnGetCurrencySuccess, OnGetCurrencyError);

    }

    private void OnGetCurrencySuccess(GetInventoryItemsResponse response)
    {

        foreach (InventoryItem item in response.Items)
        {
            if (item.Id == _goldId)
            {
                ToolCurrencies.goldAmount = (int)item.Amount;

            }
            else if (item.Id == _crystalsId)
            {
                ToolCurrencies.crystalsAmount = (int)item.Amount;
            }


        }
    }

    private void OnGetCurrencyError(PlayFabError error)
    {
        Debug.LogError(error.ErrorMessage);
    }

    public void GetEnergy()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest()
        {
        }, OnGetEnergySuccess, OnGetEnergyError);

    }

    private void OnGetEnergySuccess(GetUserInventoryResult result)
    {
        Debug.Log(result.VirtualCurrency["EN"]);
    }

    private void OnGetEnergyError(PlayFabError error)
    {
    }
}
