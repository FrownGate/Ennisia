using System;
using System.Collections.Generic;
using System.Collections;
using PlayFab;
using PlayFab.GroupsModels;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    public enum GameCurrency //TODO -> move elsewhere
    {
        Gold, Crystals, Fragments, EternalKeys, TerritoriesCurrency
    }

    public static PlayFabManager Instance { get; private set; }

    //Requests events
    public static event Action<PlayFabError> OnError;
    public static event Action<string> OnSuccessMessage;

    //Loading events
    public static event Action OnLoadingStart;
    public static event Action OnBigLoadingStart;
    public static event Action OnLoadingEnd;

    //Account Module
    [SerializeField] private AccountModule _accountMod;

    public static event Action OnLoginSuccess;

    public Data Data => _accountMod.Data;
    public AccountData Account => _accountMod.Data.Account;
    public PlayerData Player => _accountMod.Data.Player;
    public InventoryData Inventory => _accountMod.Data.Inventory;
    public string PlayFabId => _accountMod.PlayFabId;
    public PlayFab.ClientModels.EntityKey Entity => _accountMod.Entity;
    public bool LoggedIn => _accountMod.IsLoggedIn;
    public bool IsFirstLogin => _accountMod.IsFirstLogin;

    //Economy Module
    [SerializeField] private EconomyModule _economyMod;

    public static event Action OnCurrencyUpdate;
    public static event Action<GameCurrency> OnCurrencyUsed;
    public static event Action<GameCurrency> OnCurrencyGained;
    public static event Action OnEnergyUpdate;
    public static event Action OnEnergyUsed;

    public Dictionary<GameCurrency, int> Currencies => _economyMod.Currencies;
    public int Energy => _economyMod.Energy;

    //Guilds Module
    [SerializeField] private GuildsModule _guildsMod;

    public static event Action<List<GroupWithRoles>> OnGetGuilds;
    public static event Action<GuildData, List<EntityMemberRole>> OnGetGuildData;
    public static event Action<List<GroupApplication>> OnGetApplications;
    public static event Action<List<GroupInvitation>> OnGetInvitations;

    public GroupWithRoles PlayerGuild => _guildsMod.PlayerGuild;
    public GuildData PlayerGuildData => _guildsMod.PlayerGuildData;
    public List<EntityMemberRole> PlayerGuildMembers => _guildsMod.PlayerGuildMembers;

    //Requests
    private int _requests;

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

            AccountModule.OnInitComplete += _economyMod.GetEconomyData;
            EconomyModule.OnInitComplete += _guildsMod.GetPlayerGuild;
            GuildsModule.OnInitComplete += _accountMod.CompleteLogin;

            _requests = 0;
        }
    }

    private void OnDestroy()
    {
        AccountModule.OnInitComplete -= _economyMod.GetEconomyData;
        EconomyModule.OnInitComplete -= _guildsMod.GetPlayerGuild;
        GuildsModule.OnInitComplete -= _accountMod.CompleteLogin;
    }

    private void Start()
    {
        OnBigLoadingStart?.Invoke();
        _accountMod.StartLogin();
    }

    #region Account
    public void InvokeOnLoginSuccess() => OnLoginSuccess?.Invoke();

    public void UpdateData() => StartCoroutine(_accountMod.UpdateData());
    public void SetGender(int gender) => _accountMod.SetGender(gender);
    #endregion

    #region Economy
    public void InvokeOnCurrencyUpdate() => OnCurrencyUpdate?.Invoke();
    public void InvokeOnCurrencyUsed(GameCurrency currency) => OnCurrencyUsed?.Invoke(currency);
    public void InvokeOnCurrencyGained(GameCurrency currency) => OnCurrencyGained?.Invoke(currency);
    public void InvokeOnEnergyUpdate() => OnEnergyUpdate?.Invoke();
    public void InvokeOnEnergyUsed() => OnEnergyUsed?.Invoke();

    public void AddCurrency(GameCurrency currency, int amount) => StartCoroutine(_economyMod.AddCurrency(currency, amount));
    public void RemoveCurrency(GameCurrency currency, int amount) => StartCoroutine(_economyMod.RemoveCurrency(currency, amount));
    public void AddEnergy(int amount) => StartCoroutine(_economyMod.AddEnergy(amount));
    public void RemoveEnergy(int amount) => StartCoroutine(_economyMod.RemoveEnergy(amount));
    public bool IsEnergyUsed(int amount) => _economyMod.EnergyIsUsed(amount);

    public void AddInventoryItem(Item item) => StartCoroutine(_economyMod.AddInventoryItem(item));
    public void UpdateItem(Item item) => StartCoroutine(_economyMod.UpdateItem(item));
    public void UseItem(Item item, int amount = 1) => StartCoroutine(_economyMod.UseItem(item, amount));
    #endregion

    #region Guilds
    public void InvokeOnGetGuilds(List<GroupWithRoles> guilds) => OnGetGuilds?.Invoke(guilds);
    public void InvokeOnGetGuildData(GuildData guild, List<EntityMemberRole> members) => OnGetGuildData?.Invoke(guild, members);
    public void InvokeOnGetApplications(List<GroupApplication> applications) => OnGetApplications?.Invoke(applications);
    public void InvokeOnGetInvitations(List<GroupInvitation> invitations) => OnGetInvitations?.Invoke(invitations);

    public void CreateGuild(string name, string description) => StartCoroutine(_guildsMod.CreateGuild(name, description));
    public void UpdatePlayerGuild() => StartCoroutine(_guildsMod.UpdatePlayerGuild());
    public void GetGuildData(GroupWithRoles guild) => StartCoroutine(_guildsMod.GetGuildData(guild));
    public void GetGuilds() => StartCoroutine(_guildsMod.GetGuilds());
    public void GetPlayerOpportunities() => StartCoroutine(_guildsMod.GetPlayerOpportunities());
    public void ApplyToGuild(GroupWithRoles guild) => StartCoroutine(_guildsMod.ApplyToGuild(guild));
    public void GetGuildApplications() => StartCoroutine(_guildsMod.GetGuildApplications());
    public void AcceptGuildApplication(string applicant) => StartCoroutine(_guildsMod.AcceptGuildApplication(applicant));
    public void DenyGuildApplication(string applicant) => StartCoroutine(_guildsMod.DenyGuildApplication(applicant));
    public void SendGuildInvitation(string username) => StartCoroutine(_guildsMod.SendGuildInvitation(username));
    public void GetGuildInvitations() => StartCoroutine(_guildsMod.GetGuildInvitations());
    public void AcceptGuildInvitation(string guild) => StartCoroutine(_guildsMod.AcceptGuildInvitation(guild));
    public void DenyGuildInvitation(string guild) => StartCoroutine(_guildsMod.DenyGuildInvitation(guild));
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
        int currentRequest = _requests;
        _requests++;
        OnLoadingStart?.Invoke();
        if (!string.IsNullOrEmpty(log)) Debug.Log(log);
        return currentRequest;
    }

    public void EndRequest(string log = null)
    {
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
        //_requests = 0;

        //AddInventoryItem(new Material(Item.ItemCategory.Weapon, Item.ItemRarity.Common, 5));
        //AddInventoryItem(new Material(Item.ItemCategory.Weapon, Item.ItemRarity.Legendary, 5));

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