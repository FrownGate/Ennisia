using PlayFab.DataModels;
using PlayFab.Internal;
using PlayFab;
using UnityEngine;
using System.Text;
using System;
using PlayFab.ClientModels;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class AccountModule : Module
{
    public static event Action OnInitComplete;
    public Data Data { get; private set; } //Account, Player and Inventory datas
    public string PlayFabId { get; private set; }
    public PlayFab.ClientModels.EntityKey Entity { get; private set; }
    public bool IsLoggedIn { get; private set; }
    public bool IsFirstLogin { get; private set; }
    public readonly Dictionary<Attribute, float> PlayerBaseStats = new();

    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path; //Local save path

    //TODO -> event when file upload is finished, prevent double uploads

    private void Awake()
    {
        _authData = new();
        _binaryFormatter = new();
        _path = Application.persistentDataPath + "/ennisia.save";
        Debug.Log($"Your save path is : {_path}");
        IsLoggedIn = false;
    }

    public void StartLogin()
    {
        //if (HasLocalSave()) return;
        //AnonymousLogin();

        //Use this line instead of AnonymousLogin to test PlayFab Login with no local save
        Login("testing@gmail.com", "testing");
    }

    #region Local Save
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

    private void CreateAccountData(string email, string password)
    {
        //Create binary file with user datas
        _authData = new()
        {
            Email = email,
            Password = password
        };
    }
    #endregion

    #region Login
    private void Login(string email, string password)
    {
        _manager.StartRequest();

        if (_authData == null) CreateAccountData(email, password);

        PlayFabClientAPI.LoginWithEmailAddress(new()
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new() { GetUserAccountInfo = true }
        }, OnLoginRequestSuccess, OnLoginRequestError);
    }

    private void AnonymousLogin()
    {
        _manager.StartRequest("No local save found -> anonymous login...");

        PlayFabClientAPI.LoginWithCustomID(new()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new() { GetUserAccountInfo = true }
        }, OnLoginRequestSuccess, _manager.OnRequestError);
    }

    private void OnLoginRequestSuccess(LoginResult result)
    {
        PlayFabId = result.PlayFabId;
        Entity = result.EntityToken.Entity;
        Debug.Log(_manager.Entity.Id);

        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        IsFirstLogin = info.Created == info.TitleInfo.Created && result.LastLoginTime == null;

        _manager.EndRequest("Login request success !");

        CreateLocalData(CreateUsername());
        StartCoroutine(GetPlayerBaseStats());

        if (HasAuthData()) CreateSave();

        if (!IsFirstLogin)
        {
            GetAccountData();
            return;
        }

        OnInitComplete?.Invoke();

        //Use this line once to test PlayFab Register & Login
        //RegisterAccount("testing@gmail.com", "Testing");
    }

    private bool HasAuthData()
    {
        return !string.IsNullOrEmpty(_authData.Email) && !string.IsNullOrEmpty(_authData.Email);
    }

    private void OnLoginRequestError(PlayFabError error)
    {
        _manager.OnRequestError(error);
        _authData ??= null;

        if (File.Exists(_path)) File.Delete(_path);
    }
    #endregion

    private IEnumerator GetPlayerBaseStats()
    {
        yield return _manager.StartAsyncRequest("Getting player base stats...");

        PlayFabClientAPI.GetTitleData(new(), res =>
        {
            foreach (var data in res.Data)
            {
                if (Enum.TryParse(data.Key, out Attribute attribute))
                {
                    PlayerBaseStats[attribute] = float.Parse(data.Value);
                }
            }

            Data.Player.UpdatePlayerStats();
            _manager.EndRequest("Player base stats obtained !");
        }, _manager.OnRequestError);
    }

    public void GetAccountData()
    {
        _manager.StartRequest("Getting user files...");

        PlayFabDataAPI.GetFiles(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type }
        }, res =>
        {
            _manager.EndRequest($"Obtained {res.Metadata.Count} file(s) !");

            if (res.Metadata.Count == 0)
            {
                Debug.LogWarning("Missing datas - creating ones...");
                StartCoroutine(UpdateData());
                OnInitComplete?.Invoke();
                return;
            }

            GetFilesDatas(res.Metadata[Data.GetType().Name]);
        }, _manager.OnRequestError);
    }

    private void GetFilesDatas(GetFileMetadata file)
    {
        _manager.StartRequest();

        PlayFabHttp.SimpleGetCall(file.DownloadUrl, res =>
        {
            //TODO -> check if there's no missing datas
            Data.UpdateLocalData(Encoding.UTF8.GetString(res));
            _manager.EndRequest("Local datas updated !");
            OnInitComplete?.Invoke();
        }, error => Debug.LogError(error));
    }

    public IEnumerator UpdateData()
    {
        yield return _manager.StartAsyncRequest("Initiating data update...");

        PlayFabDataAPI.InitiateFileUploads(new()
        {
            Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
            FileNames = new() { Data.GetType().Name }
        }, res =>
        {
            PlayFabHttp.SimplePutCall(res.UploadDetails[0].UploadUrl, Data.Serialize(), success =>
            {
                PlayFabDataAPI.FinalizeFileUploads(new()
                {
                    Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
                    FileNames = new() { Data.GetType().Name }
                }, res => _manager.EndRequest("Files uploaded !"), _manager.OnRequestError);
            }, error => Debug.LogError(error));
        }, _manager.OnRequestError);
    }

    public void CreateLocalData(string username)
    {
        Data = new(username);
    }

    #region Utilities
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

    private void RegisterAccount(string email, string password) //This function will be registered to a button event
    {
        _manager.StartRequest("Registering account...");
        CreateAccountData(email, password);
        string username = CreateUsername(email);

        PlayFabClientAPI.AddUsernamePassword(new()
        {
            Username = username, //Create unique username with email
            Email = email,
            Password = password //Password must be between 6 and 100 characters
        },
        res => {
            _manager.EndRequest();
            UpdateName(username);
            CreateSave();
        }, _manager.OnRequestError);
    }

    public void UpdateName(string name)
    {
        _manager.StartRequest("Updating username...");

        PlayFabClientAPI.UpdateUserTitleDisplayName(new()
        {
            DisplayName = name
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }

    public void SetGender(int gender)
    {
        Data.Account.Gender = gender;
        StartCoroutine(UpdateData());
    }

    public void CompleteLogin()
    {
        if (IsFirstLogin) UpdateName(Data.Account.Name);
        IsFirstLogin = false;
        IsLoggedIn = true;
        Debug.Log("Login complete !");
        _manager.InvokeOnLoginSuccess();
        _manager.Testing();
    }
    #endregion
}