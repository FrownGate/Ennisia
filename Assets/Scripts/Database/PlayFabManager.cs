using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.EconomyModels;
using PlayFab.GroupsModels;
using PlayFab.Internal;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    //TODO -> Remove item from local inventory if database inventory isn't updated

    public enum Currency //TODO -> move elsewhere
    {
        Gold, Crystals, Fragments, EternalKeys, TerritoriesCurrency
    }

    public static PlayFabManager Instance { get; private set; }

    //Requests events
    public static event Action OnLoginSuccess;
    public static event Action<PlayFabError> OnError;
    //public static event Action<string> OnSuccessMessage;

    //Loading events
    public static event Action OnLoadingStart;
    public static event Action OnBigLoadingStart;
    public static event Action OnLoadingEnd;

    public Data Data { get; private set; } //Account, Player and Inventory datas
    public AccountData Account { get => Data.Account; }
    public PlayerData Player { get => Data.Player; }
    public InventoryData Inventory { get => Data.Inventory; }

    //PlayFab Account datas
    public string PlayFabId { get; private set; }
    public PlayFab.ClientModels.EntityKey Entity { get; private set; }
    public bool LoggedIn { get; private set; }

    //Economy Module
    private EconomyModule _economyMod;
    public Dictionary<Currency, int> Currencies { get => _economyMod.Currencies; }
    public int Energy { get => _economyMod.Energy; }

    //Guilds Module
    private GuildsModule _guildsMod;
    public GroupWithRoles PlayerGuild { get => _guildsMod.PlayerGuild; }
    public GuildData PlayerGuildData { get => _guildsMod.PlayerGuildData; }
    public List<EntityMemberRole> PlayerGuildMembers { get => _guildsMod.PlayerGuildMembers; }

    //Utilities
    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path; //Local save path
    private bool _firstLogin;
    private bool _currencyAdded;
    private Item _item;
    private bool _isPending;

    //TODO -> event when file upload is finished, prevent double uploads
    //TODO -> refresh ui after some events

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
            _economyMod = new(this);
            _guildsMod = new(this);

            _authData = new();
            _binaryFormatter = new();
            _path = Application.persistentDataPath + "/ennisia.save";
            Debug.Log($"Your save path is : {_path}");
            LoggedIn = false;

            _isPending = false;
        }
    }

    private void Start()
    {
        OnBigLoadingStart?.Invoke();

        if (HasLocalSave()) return;
        Debug.Log("No local save found -> anonymous login");
        AnonymousLogin();

        //Use this line instead of AnonymousLogin to test PlayFab Login with no local save
        //Login("testing@gmail.com", "Testing");
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

        PlayFabClientAPI.LoginWithEmailAddress(new()
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new() { GetUserAccountInfo = true }
        }, OnLoginRequestSuccess, OnLoginRequestError);
    }

    private void AnonymousLogin()
    {
        PlayFabClientAPI.LoginWithCustomID(new()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new() { GetUserAccountInfo = true }
        }, OnLoginRequestSuccess, OnRequestError);
    }

    private void OnLoginRequestSuccess(LoginResult result)
    {
        Debug.Log("Login request success !");
        CreateLocalData(CreateUsername());

        PlayFabId = result.PlayFabId;
        Entity = result.EntityToken.Entity;
        Debug.Log(Entity.Id);

        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        _firstLogin = info.Created == info.TitleInfo.Created && result.LastLoginTime == null;

        if (HasAuthData()) CreateSave();

        _economyMod.GetEconomyData();

        //Use this line once to test PlayFab Register & Login
        //RegisterAccount("testing@gmail.com", "Testing");
    }

    private void CreateLocalData(string username)
    {
        Data = new(username);
    }

    private void OnLoginRequestError(PlayFabError error)
    {
        OnRequestError(error);
        _authData ??= null;

        if (File.Exists(_path)) File.Delete(_path);
    }
    #endregion

    #region 3 - Currencies
    //Get game currencies
    //New player -> Add initial currencies
    //Else -> Get currencies

    private IEnumerator CreateInitialCurrencies()
    {
        foreach(KeyValuePair<Currency, int> currency in Currencies)
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
                        Value = currency.Key.ToString()
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
        GetUserFiles();
    }
    #endregion

    #region 4 - User Datas
    //Datas = Account, Player, Inventory
    //New player -> Create datas then Update
    //Else -> Get existing datas
    //Set LoggedIn as true and invoke Login event

    public void UpdateData()
    {
        Debug.Log("Initiating data update...");
        OnLoadingStart?.Invoke();

        PlayFabDataAPI.InitiateFileUploads(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            FileNames = new() { Data.GetType().Name }
        }, OnInitFileUploads, OnRequestError);
    }

    private void OnInitFileUploads(InitiateFileUploadsResponse response)
    {
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl, Data.Serialize(), success => UploadFiles(), error => Debug.LogError(error));
    }

    private void UploadFiles()
    {
        Debug.Log("Uploading files...");

        PlayFabDataAPI.FinalizeFileUploads(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            FileNames = new() { Data.GetType().Name }
        }, res =>
        {
            Debug.Log("Files uploaded !");
            OnLoadingEnd?.Invoke();
            CompleteLogin();
        }, OnRequestError);
    }

    private void GetUserFiles()
    {
        Debug.Log("Getting user files...");

        PlayFabDataAPI.GetFiles(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type }
        }, OnFileObtained, OnRequestError);
    }

    private void OnFileObtained(GetFilesResponse response)
    {
        Debug.Log($"Obtained {response.Metadata.Count} file(s) !");

        if (response.Metadata.Count == 0)
        {
            Debug.LogWarning("Missing datas - creating ones...");
            UpdateData();
            return;
        }
        else
        {
            GetFilesDatas(response.Metadata[Data.GetType().Name]);
        }
    }

    private void GetFilesDatas(GetFileMetadata file)
    {
        PlayFabHttp.SimpleGetCall(file.DownloadUrl, res =>
        {
            //TODO -> check if there's no missing datas
            Data.UpdateLocalData(Encoding.UTF8.GetString(res));
            Debug.Log("Local datas updated !");

            GetPlayerInventory();
        }, error => Debug.LogError(error));
    }

    public void GetPlayerInventory()
    {
        PlayFabEconomyAPI.GetInventoryItems(new()
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
                Currencies[Enum.Parse<Currency>(_currencies[item.Id])] = (int)item.Amount;
            }
            else if (item.Type == "catalogItem")
            {
                Type type = Type.GetType(_itemsById[item.Id]);
                Activator.CreateInstance(type, item);
            }
        }

        Data.UpdateEquippedGears();
        _guildsMod.GetPlayerGuild();
    }

    public void CompleteLogin()
    {
        if (!LoggedIn)
        {
            if (_firstLogin) UpdateName(Data.Account.Name);
            _firstLogin = false;
            LoggedIn = true;
            OnLoginSuccess?.Invoke();
            Debug.Log("Login complete !");
            Testing();
        }
    }
    #endregion

    #region Economy
    public static event Action OnCurrencyUpdate;
    public static event Action<Currency> OnCurrencyUsed;
    public static event Action<Currency> OnCurrencyGained;
    public static event Action OnEnergyUpdate;
    public static event Action OnEnergyUsed;

    public void InvokeOnCurrencyUpdate() => OnCurrencyUpdate?.Invoke();
    public void InvokeOnCurrencyUsed(Currency currency) => OnCurrencyUsed?.Invoke(currency);
    public void InvokeOnCurrencyGained(Currency currency) => OnCurrencyGained?.Invoke(currency);
    public void InvokeOnEnergyUpdate() => OnEnergyUpdate?.Invoke();
    public void InvokeOnEnergyUsed() => OnEnergyUsed?.Invoke();

    public void AddCurrency(Currency currency, int amount) => _economyMod.AddCurrency(currency, amount);
    public void RemoveCurrency(Currency currency, int amount) => _economyMod.RemoveCurrency(currency, amount);
    public void AddEnergy(int amount) => _economyMod.AddEnergy(amount);
    public void RemoveEnergy(int amount) => _economyMod.RemoveEnergy(amount);
    public bool EnergyIsUsed(int amount) => _economyMod.EnergyIsUsed(amount);
    #endregion

    #region Gacha
    public Dictionary<int, int> GetSupports()
    {
        Dictionary<int, int> supports = new();

        foreach (SupportData support in Data.Inventory.Supports)
        {
            supports[support.Id] = support.Lvl;
        }

        return supports;
    }

    public int HasSupport(int id)
    {
        for (int i = 0; i < Data.Inventory.Supports.Count; i++)
        {
            if (Data.Inventory.Supports[i].Id == id) return i;
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

        Data.Inventory.Supports = supports;
        UpdateData();
    }
    #endregion

    #region Items
    public IEnumerator AddInventoryItem(Item item)
    {
        yield return StartCoroutine(CheckPendingRequests());
        StartRequest();
        Debug.Log("adding item");
        item.Serialize();

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
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
        }, res => EndRequest("Item added to inventory !"), OnRequestError);
    }

    public void UpdateItem(Item item)
    {
        if (item == null || !Data.Inventory.HasItem(item))
        {
            Debug.LogError("Item not found !");
            return;
        }

        OnLoadingStart?.Invoke();
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
        }, res =>
        {
            Debug.Log("Item updated !");
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void UseItem(Item item, int amount = 1)
    {
        if (item == null || !Data.Inventory.HasItem(item))
        {
            Debug.LogError("Item not found !");
            return;
        }

        _item = item;
        OnLoadingStart?.Invoke();

        PlayFabEconomyAPI.SubtractInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
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
            Data.Inventory.RemoveItem(_item);
            Debug.Log("Item used !");
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }
    #endregion

    #region Guilds
    public static event Action<List<GroupWithRoles>> OnGetGuilds;
    public static event Action<GuildData, List<EntityMemberRole>> OnGetGuildData;
    public static event Action<List<GroupApplication>> OnGetApplications;
    public static event Action<List<GroupInvitation>> OnGetInvitations;

    public void InvokeOnGetGuilds(List<GroupWithRoles> guilds) => OnGetGuilds?.Invoke(guilds);
    public void InvokeOnGetGuildData(GuildData guild, List<EntityMemberRole> members) => OnGetGuildData?.Invoke(guild, members);
    public void InvokeOnGetApplications(List<GroupApplication> applications) => OnGetApplications?.Invoke(applications);
    public void InvokeOnGetInvitations(List<GroupInvitation> invitations) => OnGetInvitations?.Invoke(invitations);

    public void CreateGuild(string name, string description) => _guildsMod.CreateGuild(name, description);
    public void UpdatePlayerGuild() => _guildsMod.UpdatePlayerGuild();
    public void GetGuildData(GroupWithRoles guild) => _guildsMod.GetGuildData(guild);
    public void GetGuilds() => _guildsMod.GetGuilds();
    public void GetPlayerOpportunities() => _guildsMod.GetPlayerOpportunities();
    public void ApplyToGuild(GroupWithRoles guild) => _guildsMod.ApplyToGuild(guild);
    public void GetGuildApplications() => _guildsMod.GetGuildApplications();
    public void AcceptGuildApplication(string applicant) => _guildsMod.AcceptGuildApplication(applicant);
    public void DenyGuildApplication(string applicant) => _guildsMod.DenyGuildApplication(applicant);
    public void SendGuildInvitation(string username) => _guildsMod.SendGuildInvitation(username);
    public void GetGuildInvitations() => _guildsMod.GetGuildInvitations();
    public void AcceptGuildInvitation(string guild) => _guildsMod.AcceptGuildInvitation(guild);
    public void DenyGuildInvitation(string guild) => _guildsMod.DenyGuildInvitation(guild);
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
        OnLoadingStart?.Invoke();

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        }, res => OnLoadingEnd?.Invoke(), OnRequestError);
    }
    public void SetGender(int gender)
    {
        Account.Gender = gender;
        UpdateData();
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
        OnLoadingEnd?.Invoke();
    }
    #endregion

    private IEnumerator CheckPendingRequests()
    {
        Debug.Log("new request received");
        yield return new WaitUntil(() => !_isPending);
        Debug.Log("pending end");
    }

    public void StartRequest(string log = null)
    {
        Debug.Log("starting request...");
        _isPending = true;
        OnLoadingStart?.Invoke();
        if (!string.IsNullOrEmpty(log)) Debug.Log(log);
    }

    public void EndRequest(string log = null)
    {
        Debug.Log("ending request...");
        _isPending = false;
        OnLoadingEnd?.Invoke();
        if (!string.IsNullOrEmpty(log)) Debug.Log(log);
    }

    //Called after login success to test code
    private void Testing()
    {
        Debug.Log("Testing");
        //Debug.Log(Data.Inventory.Items.Count);
        //foreach (int gearId in Data.Player.EquippedGears) { Debug.Log(gearId);  }

        //UseItem(Data.Inventory.GetItem(new SummonTicket(), Item.ItemRarity.Common));
        //Data.Inventory.Items["Gear"][0].Upgrade();

        //AddInventoryItem(new Gear(Item.GearType.Boots, Item.ItemRarity.Rare));
        //AddInventoryItem(new Gear(Item.GearType.Boots, Item.ItemRarity.Legendary));

        //GearSO weapon = Resources.Load<GearSO>("SO/Weapons/Pure Innocence");
        //AddInventoryItem(new Gear(weapon, Item.ItemRarity.Legendary));
        //Gear gear = (Gear)Data.Inventory.Items["Gear"][0];

        //foreach (int gearId in Data.Player.EquippedGears) { Debug.Log(gearId); }

        StartCoroutine(AddInventoryItem(new Material(Item.ItemCategory.Weapon, Item.ItemRarity.Common, 5)));
        StartCoroutine(AddInventoryItem(new Material(Item.ItemCategory.Weapon, Item.ItemRarity.Legendary, 5)));

        //CreateGuild("Test");

        //foreach (var item in Player.EquippedGears)
        //{
        //    Debug.Log(item.Key);

        //    if (item.Value != null)
        //    {
        //        Debug.Log(item.Value.Name);
        //    }
        //}

        //SupportCharacterSO support = Resources.Load<SupportCharacterSO>("SO/SupportsCharacter/Legendary/2-Theaume");
        //Player.Equip(support);
        //Debug.Log(Player.EquippedSupports[0] != null ? Player.EquippedSupports[0].Name : "Empty");

        //Player.Unequip(0);
        //Debug.Log(Player.EquippedSupports[0] != null ? Player.EquippedSupports[0].Name : "Empty");

        //Debug.Log(Player.EquippedGears[Item.GearType.Weapon] != null ? Player.EquippedGears[Item.GearType.Weapon].Name : "Empty");
        //Player.Unequip(Item.GearType.Weapon);
        //Debug.Log(Player.EquippedGears[Item.GearType.Weapon] != null ? Player.EquippedGears[Item.GearType.Weapon].Name : "Empty");
    }
}