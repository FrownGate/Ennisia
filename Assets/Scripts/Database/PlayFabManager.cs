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
    public enum Currency //TODO -> move elsewhere
    {
        Gold, Crystals, Fragments, EternalKeys, TerritoriesCurrency
    }

    public static PlayFabManager Instance { get; private set; }

    //Requests events
    public static event Action OnLoginSuccess;
    public static event Action<PlayFabError> OnError;
    public static event Action<string> OnSuccessMessage;

    //Currencies events
    public static event Action OnCurrencyUpdate;
    public static event Action<Currency> OnCurrencyUsed;
    public static event Action<Currency> OnCurrencyGained;
    public static event Action OnEnergyUpdate;
    public static event Action OnEnergyUsed;

    //Loading events
    public static event Action OnLoadingStart;
    public static event Action OnBigLoadingStart;
    public static event Action OnLoadingEnd;

    //Guilds events
    public static event Action<List<GroupWithRoles>> OnGetGuilds;
    public static event Action<List<EntityMemberRole>> OnGetGuildMembers;
    public static event Action<List<GroupApplication>> OnGetApplications;
    public static event Action<List<GroupInvitation>> OnGetInvitations;

    public Data Data { get; private set; } //Account, Player and Inventory datas
    public AccountData Account { get => Data.Account; }
    public PlayerData Player { get => Data.Player; }
    public InventoryData Inventory { get => Data.Inventory; }
    public Dictionary<Currency, int> Currencies { get; private set; } //Player's currencies
    public int Energy { get; private set; } //Player's energy

    //PlayFab Account datas
    public string PlayFabId { get; private set; }
    public PlayFab.ClientModels.EntityKey Entity { get; private set; }
    public bool LoggedIn { get; private set; }

    //PlayFab Guilds
    public GroupWithRoles PlayerGuild { get; private set; }
    public List<EntityMemberRole> PlayerGuildMembers { get; private set; }
    private PlayFab.GroupsModels.EntityKey _guildEntity; //Current request's guild's entity
    private PlayFab.ClientModels.EntityKey _adminEntity;
    //private readonly string _fakeRole = "Fake"; //Used ?

    //PlayFab catalog
    private Dictionary<string, string> _currencies;
    private Dictionary<string, string> _itemsById;
    private Dictionary<string, string> _itemsByName;
    private struct CurrencyData { public int Initial; } //TODO -> move elsewhere ?

    //Utilities
    private AuthData _authData;
    private BinaryFormatter _binaryFormatter;
    private string _path; //Local save path
    private bool _firstLogin;
    private bool _currencyAdded;
    private Item _item;

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

            _authData = new();
            _binaryFormatter = new();
            _path = Application.persistentDataPath + "/ennisia.save";
            Debug.Log($"Your save path is : {_path}");
            LoggedIn = false;

            _adminEntity = new()
            {
                Id = "2641E2E5FB9FA5C2",
                Type = "title_player_account"
            }; //TODO -> encrypt datas
            //Can be replace by Get Account Info request
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
        Debug.Log("Login request success !");
        CreateLocalData(CreateUsername());

        PlayFabId = result.PlayFabId;
        Entity = result.EntityToken.Entity;
        Debug.Log(Entity.Id);

        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        _firstLogin = info.Created == info.TitleInfo.Created && result.LastLoginTime == null;

        if (HasAuthData()) CreateSave();

        GetEconomyData();

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

    #region 2 - Game Datas
    //Get catalog items and currencies

    private void GetEconomyData()
    {
        Debug.Log("Getting game currencies and catalog items...");
        PlayFabEconomyAPI.SearchItems(new(), OnGetDataSuccess, OnRequestError);
    }

    private void OnGetDataSuccess(SearchItemsResponse response)
    {
        _itemsById = new();
        _itemsByName = new();
        _currencies = new();
        Currencies = new();

        foreach (PlayFab.EconomyModels.CatalogItem item in response.Items)
        {
            //TODO -> Get bundle items and shops
            if (item.Type == "currency")
            {
                _currencies[item.Id] = item.AlternateIds[0].Value;
                CurrencyData data = JsonUtility.FromJson<CurrencyData>(item.DisplayProperties.ToString());
                Currencies[Enum.Parse<Currency>(_currencies[item.Id])] = data.Initial;
            }
            else if (item.Type == "catalogItem")
            {
                _itemsById[item.Id] = item.AlternateIds[0].Value;
                _itemsByName[item.AlternateIds[0].Value] = item.Id;
            }
        }

        //TODO -> check initial value of new currencies for existing players
        if (_firstLogin)
        {
            StartCoroutine(CreateInitialCurrencies());
        }
        else
        {
            GetEnergy();
        }
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
        PlayFabEconomyAPI.GetInventoryItems(new GetInventoryItemsRequest()
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
        GetPlayerGuild();
    }

    private void GetPlayerGuild()
    {
        Debug.Log("Fetching player's guild...");

        PlayFabGroupsAPI.ListMembership(new(), res =>
        {
            PlayerGuild = res.Groups.Count > 0 ? res.Groups[0] : null;

            if (PlayerGuild != null)
            {
                Debug.Log($"Player is a member of Guild {PlayerGuild.GroupName}.");
                GetGuildMembers(PlayerGuild);
                return;
            }
            
            CompleteLogin();

        }, OnRequestError);
    }

    private void CompleteLogin()
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
    public void AddCurrency(Currency currency, int amount)
    {
        Debug.Log($"Adding {amount} {currency}...");
        OnLoadingStart?.Invoke();

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency.ToString()
                }
            }
        }, res => {
            Debug.Log($"Added {amount} {currency} !");
            Currencies[currency] += amount;
            OnCurrencyUpdate?.Invoke();
            OnCurrencyGained?.Invoke(currency);
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void RemoveCurrency(Currency currency, int amount)
    {
        Debug.Log($"Removing {amount} {currency}...");
        OnLoadingStart?.Invoke();

        PlayFabEconomyAPI.SubtractInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Amount = amount,
            Item = new()
            {
                AlternateId = new()
                {
                    Type = "FriendlyId",
                    Value = currency.ToString()
                }
            }
        }, res => {
            Debug.Log($"Removed {amount} {currency} !");
            Currencies[currency] -= amount;
            OnCurrencyUpdate?.Invoke();
            OnCurrencyUsed?.Invoke(currency);
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void AddEnergy(int amount)
    {
        Debug.Log($"Adding {amount} energy...");
        OnLoadingStart?.Invoke();

        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res => {
            Debug.Log($"Added {amount} energy !");
            Energy += amount;
            OnEnergyUpdate?.Invoke();
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void RemoveEnergy(int amount)
    {
        Debug.Log($"Removing {amount} energy...");
        OnLoadingStart?.Invoke();

        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest()
        {
            Amount = amount,
            VirtualCurrency = "EN"
        }, res => {
            Debug.Log($"Removed {amount} energy !");
            Energy -= amount;
            OnEnergyUpdate?.Invoke();
            OnEnergyUsed?.Invoke();
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public bool EnergyIsUsed(int amount)
    {
        if (amount >= Energy)
        {
            RemoveEnergy(amount);
        }
        else
        {
            Debug.LogError("Not enough energy");
        }

        return amount >= Energy;
    }
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

    #region Equipment
    public void SetGearData(GearSO equipment, int id)
    {
        //TODO -> Use find
        //foreach (Gear inventoryGear in Inventory.GetGears())
        //{
        //    if (inventoryGear.Id == id)
        //    {
        //        equipment.Id = inventoryGear.Id;
        //        equipment.Name = inventoryGear.Name;
        //        equipment.Type = (Item.GearType)inventoryGear.Type;
        //        equipment.Rarity = (Item.ItemRarity)inventoryGear.Rarity; //TODO -> Update Equipment SO
        //        equipment.Attribute = (Item.AttributeStat)inventoryGear.Attribute;
        //        equipment.StatValue = inventoryGear.Value;
        //        equipment.Description = inventoryGear.Description;
        //        break;
        //    }
        //}
    }
    #endregion

    #region Items
    public void AddInventoryItem(Item item)
    {
        OnLoadingStart?.Invoke();
        item.Serialize();

        PlayFabEconomyAPI.AddInventoryItems(new()
        {
            Entity = new() { Id = Entity.Id, Type = Entity.Type },
            Item = new InventoryItemReference
            {
                AlternateId = new AlternateId
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
        }, res =>
        {
            Debug.Log($"Item added to inventory !");
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
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
            Item = new InventoryItemReference
            {
                AlternateId = new AlternateId
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
    //TODO -> Remove guild member
    //TODO -> check role for applications functions
    //TODO -> Check if player's application has been accepted in update ?

    public void CreateGuild(string name)
    {
        Debug.Log("Creating guild...");

        //TODO -> Check if name already exists

        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.CreateGroup(new()
        {
            GroupName = name
        }, OnCreateGuild, OnRequestError);
    }

    private void OnCreateGuild(CreateGroupResponse response)
    {
        Debug.Log($"Guild {response.GroupName} created !");
        _guildEntity = response.Group;

        PlayFabGroupsAPI.ApplyToGroup(new() //Adding fake player to newly created guild
        {
            Entity = new() { Id = _adminEntity.Id, Type = _adminEntity.Type },
            Group = _guildEntity
        }, res =>
        {
            PlayFabGroupsAPI.AcceptGroupApplication(new()
            {
                Entity = new() { Id = _adminEntity.Id, Type = _adminEntity.Type },
                Group = _guildEntity
            }, res =>
            {
                Debug.Log($"Fake player added to Guild {response.GroupName} !");
                GetPlayerGuild();

                //TODO -> Check if filter is better with role or entity key

                /*PlayFabGroupsAPI.CreateRole(new() //Adding fake role to filter fake player
                {
                    Group = _guildEntity,
                    RoleId = _fakeRole,
                    RoleName = _fakeRole
                }, res =>
                {
                    PlayFabGroupsAPI.ChangeMemberRole(new()
                    {
                        OriginRoleId = "members",
                        DestinationRoleId = _fakeRole,
                        Members = new() { new() { Id = _adminEntity.Id, Type = _adminEntity.Type } }
                    }, null, OnRequestError);
                }, OnRequestError);*/
            }, OnRequestError);
        }, OnRequestError);
    }

    public void GetGuilds()
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.ListMembership(new()
        {
            Entity = new() { Id = _adminEntity.Id, Type = _adminEntity.Type }
        }, res => {
            OnGetGuilds?.Invoke(res.Groups);
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void GetPlayerOpportunities() //List of applications sent and invitations received by the player
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.ListMembershipOpportunities(new(), res =>
        {
            OnGetApplications?.Invoke(res.Applications);
            OnGetInvitations?.Invoke(res.Invitations);
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void ApplyToGuild(GroupWithRoles guild)
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.ApplyToGroup(new()
        {
            Group = guild.Group
        }, res =>
        {
            OnLoadingEnd?.Invoke();
            Debug.Log("Applied successfully !");
        }, OnRequestError);
    }

    public void GetGuildApplications()
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.ListGroupApplications(new()
        {
            Group = PlayerGuild.Group
        }, res =>
        {
            OnGetApplications?.Invoke(res.Applications);
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void AcceptGuildApplication(string applicant) //String needed = ApplicantId#ApplicantType
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.AcceptGroupApplication(new()
        {
            Group = PlayerGuild.Group,
            Entity = new() { Id = applicant.Split("#")[0], Type = applicant.Split("#")[1] }
        }, res => OnLoadingEnd?.Invoke(), OnRequestError);
    }

    public void DenyGuildApplication(string applicant) //String needed = ApplicantId#ApplicantType
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.RemoveGroupApplication(new()
        {
            Group = PlayerGuild.Group,
            Entity = new() { Id = applicant.Split("#")[0], Type = applicant.Split("#")[1] }
        }, res => OnLoadingEnd?.Invoke(), OnRequestError);
    }

    public void SendGuildInvitation(string username)
    {
        OnLoadingStart?.Invoke();

        PlayFabClientAPI.GetAccountInfo(new()
        {
            TitleDisplayName = username
        }, res =>
        {
            PlayFab.ClientModels.EntityKey entity = res.AccountInfo.TitleInfo.TitlePlayerAccount;

            PlayFabGroupsAPI.InviteToGroup(new()
            {
                Entity = new() { Id = entity.Id, Type = entity.Type },
                Group = PlayerGuild.Group
            }, res =>
            {
                OnLoadingEnd?.Invoke();
                OnSuccessMessage("Invitation successfully sent !");
            }, OnRequestError);
        }, OnRequestError);
    }

    public void GetGuildInvitations() //List of invitations sent by a guild
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.ListGroupInvitations(new()
        {
            Group = PlayerGuild.Group
        }, res =>
        {
            OnGetInvitations?.Invoke(res.Invitations);
            OnLoadingEnd?.Invoke();
        }, OnRequestError);
    }

    public void AcceptGuildInvitation(string guild) //String needed = GuildId#GuildTyp
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.AcceptGroupInvitation(new()
        {
            Group = new()
            {
                Id = guild.Split("#")[0],
                Type = guild.Split("#")[1]
            }
        }, res => GetPlayerGuild(), OnRequestError);
    }

    public void DenyGuildInvitation(string guild) //String needed = GuildId#GuildTyp
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.RemoveGroupInvitation(new()
        {
            Group = new()
            {
                Id = guild.Split("#")[0],
                Type = guild.Split("#")[1]
            }
        }, res => OnLoadingEnd?.Invoke(), OnRequestError);
    }

    public void GetGuildMembers(GroupWithRoles guild)
    {
        OnLoadingStart?.Invoke();

        PlayFabGroupsAPI.ListGroupMembers(new()
        {
            Group = guild.Group
        }, res =>
        {
            PlayerGuildMembers = res.Members;
            CompleteLogin();
            OnLoadingEnd?.Invoke();
            OnGetGuildMembers?.Invoke(res.Members);
        }, OnRequestError);
    }
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

    //Called after login success to test code
    private void Testing()
    {
        //Debug.Log("Testing");
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

        //AddInventoryItem(new Material(Item.ItemCategory.Weapon, Item.ItemRarity.Legendary, 5));
        //AddInventoryItem(new SummonTicket(Item.ItemRarity.Common));

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