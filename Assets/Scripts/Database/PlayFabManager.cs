using System;
using System.Collections.Generic;
using System.Collections;
using PlayFab;
using PlayFab.GroupsModels;
using UnityEngine;
using PlayFab.ClientModels;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance { get; private set; }
    public static string continuationToken;

    //Requests events
    public static event Action<PlayFabError> OnError;
    public static event Action<string> OnSuccessMessage;

    //Loading events
    public static event Action OnLoadingStart;
    public static event Action OnBigLoadingStart;
    public static event Action OnLoadingEnd;
    public static event Action OnRequest;
    public static event Action OnEndRequest;

    //Game events
    public static event Action OnObsoleteVersion;

    //Account Module
    [SerializeField] private AccountModule _accountMod;

    public static event Action OnLoginSuccess;
    public static event Action<string> OnLocalDatasChecked;

    public Data Data => _accountMod.Data;
    public AccountData Account => _accountMod.Data.Account;
    public PlayerData Player => _accountMod.Data.Player;
    public InventoryData Inventory => _accountMod.Data.Inventory;
    public string PlayFabId => _accountMod.PlayFabId;
    public PlayFab.ClientModels.EntityKey Entity => _accountMod.Entity;
    public bool LoggedIn => _accountMod.IsLoggedIn;
    public bool IsFirstLogin => _accountMod.IsFirstLogin;
    public bool IsAccountReset => _accountMod.IsAccountReset;
    public Dictionary<Attribute, float> PlayerBaseStats => _accountMod.PlayerBaseStats;

    //Economy Module
    [SerializeField] private EconomyModule _economyMod;

    public static event Action OnCurrencyUpdate;
    public static event Action<Currency, int> OnCurrencyUsed;
    public static event Action<Currency, int> OnCurrencyGained;
    public static event Action OnEnergyUpdate;
    public static event Action<int> OnEnergyUsed;

    public Dictionary<Currency, int> Currencies => _economyMod.Currencies;
    public Dictionary<string, PlayFab.EconomyModels.CatalogItem> Stores => _economyMod.Stores;
    public List<PlayFab.EconomyModels.CatalogItem> Items => _economyMod.CatalogItems;
    public int Energy => _economyMod.Energy;
    public float RatioUpgrade => _economyMod.RatioUpgrade;
    public float RatioUpgradeSubStat => _economyMod.RatioUpgradeSubStat;

    public PlayFab.EconomyModels.CatalogItem GetItemById(string id) => _economyMod.GetItemById(id);

    //Guilds Module
    [SerializeField] private GuildsModule _guildsMod;

    public static event Action<List<GroupWithRoles>> OnGetGuilds;
    public static event Action<GuildData, List<EntityMemberRole>> OnGetGuildData;
    public static event Action<List<GroupApplication>> OnGetApplications;
    public static event Action<List<GroupInvitation>> OnGetInvitations;

    public GroupWithRoles PlayerGuild => _guildsMod.PlayerGuild;
    public GuildData PlayerGuildData => _guildsMod.PlayerGuildData;
    public List<EntityMemberRole> PlayerGuildMembers => _guildsMod.PlayerGuildMembers;

    //Social Module
    [SerializeField] private SocialModule _socialMod;

    public static event Action<List<FriendInfo>> OnGetFriends;

    //Summon Module
    [SerializeField] private SummonModule _summonMod;

    public int SummonCost => _summonMod.SummonCost;
    public int FragmentsGain => _summonMod.FragmentsGain;
    public Dictionary<Rarity, double> Chances => _summonMod.Chances;

    //Requests
    private int _requests;
    public string Token;

    //TODO -> refresh ui after some events

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

            //Init all modules
            _accountMod.Init(this);
            _economyMod.Init(this);
            _guildsMod.Init(this);
            _socialMod.Init(this);
            _summonMod.Init(this);

            AccountModule.OnInitComplete += _economyMod.GetEconomyData;
            EconomyModule.OnInitComplete += _accountMod.CompleteLogin;

            //TODO -> Fix Guilds Module
            //EconomyModule.OnInitComplete += _guildsMod.GetPlayerGuild;
            //GuildsModule.OnInitComplete += _accountMod.CompleteLogin;

            _requests = 0;
            Token = null;
        }
    }

    private void OnDestroy()
    {
        AccountModule.OnInitComplete -= _economyMod.GetEconomyData;
        EconomyModule.OnInitComplete -= _accountMod.CompleteLogin;
        //EconomyModule.OnInitComplete -= _guildsMod.GetPlayerGuild;
        //GuildsModule.OnInitComplete -= _accountMod.CompleteLogin;
    }

    private void Start()
    {
        CheckGameVersion();
    }

    public void InvokeOnBigLoadingStart() => OnBigLoadingStart?.Invoke();

    #region Account

    public void InvokeOnLoginSuccess() => OnLoginSuccess?.Invoke();
    public void InvokeOnLocalDatasChecked(string username) => OnLocalDatasChecked?.Invoke(username);

    public void Login() => _accountMod.Login();
    public void Login(string email, string password) => _accountMod.Login(email, password);
    public void UpdateData() => StartCoroutine(_accountMod.UpdateData());
    public void SetGender(int gender) => _accountMod.SetGender(gender);
    public void ResetAccount(bool admin = false) => _accountMod.ResetAccount(admin);

    #endregion

    #region Economy

    public void InvokeOnCurrencyUpdate() => OnCurrencyUpdate?.Invoke();
    public void InvokeOnCurrencyUsed(Currency currency, int amount) => OnCurrencyUsed?.Invoke(currency, amount);
    public void InvokeOnCurrencyGained(Currency currency, int amount) => OnCurrencyGained?.Invoke(currency, amount);
    public void InvokeOnEnergyUpdate() => OnEnergyUpdate?.Invoke();
    public void InvokeOnEnergyUsed(int amount) => OnEnergyUsed?.Invoke(amount);

    public void AddCurrency(Currency currency, int amount) => StartCoroutine(_economyMod.AddCurrency(currency, amount));
    public void RemoveCurrency(Currency currency, int amount) => StartCoroutine(_economyMod.RemoveCurrency(currency, amount));
    public void AddEnergy(int amount) => StartCoroutine(_economyMod.AddEnergy(amount));
    public void RemoveEnergy(int amount) => StartCoroutine(_economyMod.RemoveEnergy(amount));
    public bool HasEnoughCurrency(int amount, Currency? currency = null) => _economyMod.HasEnoughCurrency(amount, currency);

    public List<Item> GetItems(Item category = null) => _economyMod.GetItems(category);

    public void AddInventoryItem(Item item) => StartCoroutine(_economyMod.AddInventoryItem(item));
    public void UpdateItem(Item item) => StartCoroutine(_economyMod.UpdateItem(item));
    public void UseItem(Item item, int amount = 1) => StartCoroutine(_economyMod.UseItem(item, amount));
    public void PurchaseInventoryItem(Item item) => StartCoroutine(_economyMod.PurchaseInventoryItem(item));

    #endregion

    #region Guilds

    public void InvokeOnGetGuilds(List<GroupWithRoles> guilds) => OnGetGuilds?.Invoke(guilds);

    public void InvokeOnGetGuildData(GuildData guild, List<EntityMemberRole> members) =>
        OnGetGuildData?.Invoke(guild, members);

    public void InvokeOnGetApplications(List<GroupApplication> applications) => OnGetApplications?.Invoke(applications);
    public void InvokeOnGetInvitations(List<GroupInvitation> invitations) => OnGetInvitations?.Invoke(invitations);

    public void CreateGuild(string name, string description) =>
        StartCoroutine(_guildsMod.CreateGuild(name, description));

    public void UpdatePlayerGuild() => StartCoroutine(_guildsMod.UpdatePlayerGuild());
    public void GetGuildData(GroupWithRoles guild) => StartCoroutine(_guildsMod.GetGuildData(guild));
    public void GetGuilds() => StartCoroutine(_guildsMod.GetGuilds());
    public void GetPlayerOpportunities() => StartCoroutine(_guildsMod.GetPlayerOpportunities());
    public void ApplyToGuild(GroupWithRoles guild) => StartCoroutine(_guildsMod.ApplyToGuild(guild));
    public void GetGuildApplications() => StartCoroutine(_guildsMod.GetGuildApplications());

    public void AcceptGuildApplication(string applicant) =>
        StartCoroutine(_guildsMod.AcceptGuildApplication(applicant));

    public void DenyGuildApplication(string applicant) => StartCoroutine(_guildsMod.DenyGuildApplication(applicant));
    public void SendGuildInvitation(string username) => StartCoroutine(_guildsMod.SendGuildInvitation(username));
    public void GetGuildInvitations() => StartCoroutine(_guildsMod.GetGuildInvitations());
    public void AcceptGuildInvitation(string guild) => StartCoroutine(_guildsMod.AcceptGuildInvitation(guild));
    public void DenyGuildInvitation(string guild) => StartCoroutine(_guildsMod.DenyGuildInvitation(guild));
    public void KickPlayer(PlayFab.GroupsModels.EntityKey player) => StartCoroutine(_guildsMod.KickPlayer(player));

    #endregion

    #region Social

    public void InvokeOnGetFriends(List<FriendInfo> friends) => OnGetFriends?.Invoke(friends);

    public void GetFriends() => StartCoroutine(_socialMod.GetFriends());
    public void AddFriend(string username) => StartCoroutine(_socialMod.AddFriend(username));
    public void RemoveFriend(string id) => StartCoroutine(_socialMod.RemoveFriend(id));

    #endregion

    #region Summon

    public Dictionary<int, int> GetSupports() => _summonMod.GetSupports();
    public int HasSupport(int id) => _summonMod.HasSupport(id);
    public void AddSupports(Dictionary<int, int> pulledSupports) => _summonMod.AddSupports(pulledSupports);

    #endregion

    #region Game
    public void CheckGameVersion()
    {
        StartRequest();

        PlayFabClientAPI.LoginWithCustomID(new()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = false
        }, res =>
        {
            PlayFabClientAPI.GetTitleData(new(), res =>
            {
                string version = res.Data["Version"];
                string localVersion = Application.version;

                EndRequest($"Last game version available : {version}");

                if (version != localVersion)
                {
                    Debug.LogWarning($"Local version obsolete : {localVersion}");
                    OnObsoleteVersion?.Invoke();
                    //TODO -> Add popup
                    return;
                }

                Debug.Log("Game version up to date !");
                _accountMod.CheckLocalDatas();
            }, OnRequestError);
        }, OnRequestError);
    }
    #endregion

    #region Requests

    public void OnRequestError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        OnError?.Invoke(error);
        OnLoadingEnd?.Invoke();
    }

    public IEnumerator StartAsyncRequest(string log = null)
    {
        int request = StartRequest(log);
        //Debug.Log($"new async request received - {_requests} requests remaining.");
        if (_requests > 1) yield return new WaitUntil(() => _requests == request);
        //Debug.Log($"starting async request... - {_requests} requests remaining.");
    }

    public int StartRequest(string log = null)
    {
        OnRequest?.Invoke();
        int currentRequest = _requests;
        _requests++;
        OnLoadingStart?.Invoke();
        if (!string.IsNullOrEmpty(log)) Debug.Log(log);
        return currentRequest;
    }

    public void EndRequest(string log = null)
    {
        OnEndRequest?.Invoke();
        //Debug.Log("ending request...");
        _requests--;
        OnLoadingEnd?.Invoke();

        if (!string.IsNullOrEmpty(log))
        {
            Debug.Log(log);
            OnSuccessMessage?.Invoke(log);
        }
    }

    #endregion

    //Called after login success to test code
    public void Testing()
    {
        Debug.LogWarning("TESTING");
        //EquipTest();
        //Gear gear = Inventory.GetGearById(1);
        //Player.Equip(gear);

        // SupportCharacterSO support = Resources.Load<SupportCharacterSO>("SO/SupportsCharacter/Legendary/Hem-Mily");
        // Debug.Log(support.Name);
        // Player.Equip(support, 0, false); //Slot 1
        //
        // support = Resources.Load<SupportCharacterSO>("SO/SupportsCharacter/Legendary/Ugho");
        // Debug.Log(support.Name);
        // Player.Equip(support, 1, false); //Slot 2
    }

    private void EquipTest()
    {
        List<Gear> gears = new();

        foreach (GearType type in Enum.GetValues(typeof(GearType)))
        {
            if (type == GearType.Weapon) continue;
            Gear gear = new(type, Rarity.Legendary, null);
            AddInventoryItem(gear);
            gears.Add(gear);
        }

        Player.Equip(gears);
    }
}