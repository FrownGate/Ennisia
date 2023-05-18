using PlayFab;
using PlayFab.ClientModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using PlayFab.DataModels;
using System.Runtime.InteropServices.ComTypes;

public class PlayFabManager : MonoBehaviour 
{
    public static PlayFabManager Instance { get; private set; }
    public static event Action OnLoginSuccess;
    public static event Action<PlayFabError> OnError;

    public struct Account
    {
        public int Level;
        public int AccountExp;
        public int Gender;
        public bool Tutorial;
        public int CharacterLevel;
        public int CharacterExp;
    }

    public Account AccountData { get; private set; }
    public int PlayFabId { get; private set; }

    public int[] EquippedGears { get; private set; }
    public int[] EquippedSupports { get; private set; }

    private List<SetObject> _dataObjects;
    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path;

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
            Password = password
        }, OnLoginRequestSuccess, OnLoginRequestError);
    }

    private void AnonymousLogin()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        }, OnLoginRequestSuccess, OnError);
    }

    private void OnLoginRequestSuccess(LoginResult result)
    {
        Debug.Log("login success");
        OnLoginSuccess?.Invoke();
        if (_authData != null) CreateSave();

        //Use this line once to test PlayFab Register & Login
        //RegisterAccount("testing@gmail.com", "Testing");
    }

    private void OnLoginRequestError(PlayFabError error)
    {
        OnRequestError(error);
        _authData ??= null;

        if (File.Exists(_path)) File.Delete(_path);
    }

    private void OnRequestError(PlayFabError error)
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

    private string CreateUsername(string email)
    {
        string name = email.Split('@')[0];
        string id = SystemInfo.deviceUniqueIdentifier[..5];
        Debug.Log($"creating username {name}{id}");
        return name + id;
    }

    private void GetUserDatas()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            //
        }, null, OnRequestError);
    }

    private void SaveUserDatas()
    {
        PlayFabDataAPI.SetObjects(new SetObjectsRequest
        {
            Objects = _dataObjects
        }, OnDataUpdate, OnRequestError);

        _dataObjects.Clear();
    }

    private void UpdateData()
    {
        _dataObjects.Add(new SetObject()
        {
            ObjectName = "Account",
            EscapedDataObject = JsonUtility.ToJson(AccountData)
        });
        SaveUserDatas();
    }

    private void CreateAccountData()
    {
        //
    }

    private void OnDataUpdate(SetObjectsResponse response)
    {
        //
    }
}
