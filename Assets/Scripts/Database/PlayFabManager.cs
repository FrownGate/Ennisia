using PlayFab;
using PlayFab.ClientModels;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayFabManager : MonoBehaviour 
{
    public static PlayFabManager Instance { get; private set; }

    private BinaryFormatter _binaryFormatter;
    private string _path;
    private string _username;
    private string _password;

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
        AccountData account = (AccountData)_binaryFormatter.Deserialize(file);
        file.Close();

        _username = account.username;
        _password = account.password;

        Login();
        ClearData();
        Debug.Log("save loaded");
        return true;
    }

    private void Login()
    {
        LoginWithPlayFabRequest request = new()
        {
            Username = _username,
            Password = _password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);
    }

    private void AnonymousLogin()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        LoginWithCustomIDRequest request = new()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("success");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void RegisterAccount()
    {
        //This function will be registered to a button event
        //Password must be between 6 and 100 characters
        _username = "Testing"; //For testing only
        _password = "Testing"; //For testing only
        string email = "testing@gmail.com"; //For testing only

        AddUsernamePasswordRequest request = new()
        {
            Username = _username,
            Password = _password,
            Email = email
        };

        PlayFabClientAPI.AddUsernamePassword(request, OnRegisterSuccess, OnError, null);
    }
    private void OnRegisterSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("success");
        CreateAccount();
    }

    private void CreateAccount()
    {
        //Create binary file with user datas
        AccountData account = new()
        {
            username = _username,
            password = _password
        };

        FileStream file = File.Create(_path);
        _binaryFormatter.Serialize(file, account);
        file.Close();
        ClearData();
    }

    private void ClearData()
    {
        _username = null;
        _password = null;
    }
}
