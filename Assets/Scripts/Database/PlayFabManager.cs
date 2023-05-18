using PlayFab;
using PlayFab.ClientModels;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayFabManager : MonoBehaviour 
{
    public static PlayFabManager Instance { get; private set; }
    public static event Action OnLoginSuccess;
    public static event Action<PlayFabError> OnError;

    public struct Account
    {
        public int Level;
        public int Gender;
        public bool Tutorial;
    }

    public struct Player
    {
        //
    }

    public int[] EquippedGears { get; private set; }
    public int[] EquippedSupports { get; private set; }

    private AccountData _accountData;
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
        Debug.Log("save found");

        using (FileStream file = new(_path, FileMode.Open))
        {
            _accountData = (AccountData)_binaryFormatter.Deserialize(file);
        }

        Login(_accountData.email, _accountData.password);
        return true;
    }

    private void Login(string email, string password)
    {
        if (_accountData == null) CreateAccountData(email, password);

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
        if (_accountData != null) CreateSave();

        //Use this line once to test PlayFab Register & Login
        //RegisterAccount("testing@gmail.com", "Testing");
    }

    private void OnLoginRequestError(PlayFabError error)
    {
        OnRequestError(error);
        _accountData ??= null;

        if (File.Exists(_path)) File.Delete(_path);
    }

    private void OnRequestError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        OnError?.Invoke(error);
    }

    private void RegisterAccount(string email, string password)
    {
        CreateAccountData(email, password);
        //This function will be registered to a button event
        string username = CreateUsername(email); //Create unique username with email

        PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest()
        {
            Username = username,
            Email = email,
            Password = password //Password must be between 6 and 100 characters
        },
        res => { CreateSave(); }, OnRequestError);
    }

    private void CreateAccountData(string email, string password)
    {
        //Create binary file with user datas
        _accountData = new()
        {
            email = email,
            password = password
        };
    }

    private void CreateSave()
    {
        using (FileStream file = new(_path, FileMode.OpenOrCreate))
        {
            _binaryFormatter.Serialize(file, _accountData);
        }

        _accountData = null;
    }

    private string CreateUsername(string email)
    {
        return "Testing";
    }
}
