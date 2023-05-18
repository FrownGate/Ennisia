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
            Debug.Log(_path);

            if (HasSave()) return;
            AnonymousLogin();
        }
    }

    private bool HasSave()
    {
        //Check if binary file with user datas exists
        if (!File.Exists(_path)) return false;

        Debug.Log("save found");
        FileStream file = File.Open(_path, FileMode.Open);
        _accountData = (AccountData)_binaryFormatter.Deserialize(file);
        file.Close();

        Login();
        return true;
    }

    private void Login()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
        {
            Username = _accountData.username,
            Password = _accountData.password
        }, OnLoginRequestSuccess, OnRequestError);
    }

    private void AnonymousLogin()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        }, OnLoginRequestSuccess, OnRequestError);
    }

    private void OnLoginRequestSuccess(LoginResult result)
    {
        Debug.Log("login success");
        OnLoginSuccess?.Invoke();
        if (_accountData == null) return;
        ClearData();
    }

    private void OnRequestError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        OnError?.Invoke(error);
    }

    private void RegisterAccount(string email, string password)
    {
        //This function will be registered to a button event
        string username = CreateUsername(email); //Create unique username with email

        PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest()
        {
            Username = username,
            Email = email,
            Password = password //Password must be between 6 and 100 characters
        },
        res =>
        {
            //Create binary file with user datas
            _accountData = new()
            {
                username = username,
                password = password
            };

            FileStream file = File.Create(_path);
            _binaryFormatter.Serialize(file, _accountData);
            file.Close();
            ClearData();
        }, OnRequestError);
    }

    private string CreateUsername(string email)
    {
        return "Testing";
    }

    private void ClearData()
    {
        _accountData = null;
    }
}
