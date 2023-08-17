using PlayFab;
using PlayFab.GroupsModels;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class GuildsModule : Module
{
    public static event Action OnInitComplete;
    public EntityKey PlayerGuild { get; private set; }
    public string PlayerGuildName { get; private set; }
    public GuildData PlayerGuildData { get; private set; }
    public List<EntityMemberRole> PlayerGuildMembers { get; private set; }
    public readonly int GuildPrice = 100000;

    private EntityKey _guildEntity; //Current request's guild's entity
    private PlayFab.ClientModels.EntityKey _adminEntity;
    private bool _creatingGuild;
    //private readonly string _fakeRole = "Fake"; //Used ?

    //TODO -> Remove guild member
    //TODO -> check role for applications functions
    //TODO -> Check if player's application has been accepted in update ?
    //TODO -> reset applications on reset

    private void Awake()
    {
        _creatingGuild = false;

        _adminEntity = new()
        {
            Id = "2641E2E5FB9FA5C2",
            Type = "title_player_account"
        }; //TODO -> encrypt datas
           //Can be replace by Get Account Info request
    }

    public void GetPlayerGuild()
    {
        //TODO -> reset guild if account reset
        _manager.StartRequest("Fetching player's guild...");

        PlayFabGroupsAPI.ListMembership(new(), res =>
        {
            _manager.EndRequest();
            PlayerGuild = res.Groups.Count > 0 ? res.Groups[0].Group : null;

            if (PlayerGuild != null)
            {
                PlayerGuildName = res.Groups[0].GroupName;
                Debug.Log($"Player is a member of Guild {PlayerGuildName}.");
                StartCoroutine(GetGuildData(PlayerGuild));
                return;
            }

            Debug.Log("Player has no Guild.");
            if (!_manager.LoggedIn) OnInitComplete?.Invoke();

        }, _manager.OnRequestError);
    }

    public IEnumerator GetGuildData(EntityKey guild)
    {
        yield return _manager.StartAsyncRequest("Getting guild members...");

        List<EntityMemberRole> members = new();

        PlayFabGroupsAPI.ListGroupMembers(new()
        {
            Group = guild
        }, res =>
        {
            members = res.Members;
            PlayerGuildMembers = guild == PlayerGuild ? members : PlayerGuildMembers;

            if (!_manager.LoggedIn && _manager.IsAccountReset)
            {
                Debug.Log("Guild reset...");
                StartCoroutine(ResetGuild());
                return;
            }

            Debug.Log("Getting guild data...");

            PlayFabDataAPI.GetObjects(new()
            {
                Entity = new() { Id = guild.Id, Type = guild.Type },
                EscapeObject = true
            }, res =>
            {
                _manager.EndRequest();
                GuildData data = new(res.Objects[new GuildData().GetType().Name]);
                PlayerGuildData = guild == PlayerGuild ? data : PlayerGuildData;

                _manager.InvokeOnGetGuildData(data, members);

                if (_creatingGuild)
                {
                    _creatingGuild = false;
                    ScenesManager.Instance.SetScene("PlayerGuild");
                    return;
                }

                if (!_manager.LoggedIn) OnInitComplete?.Invoke();
            }, _manager.OnRequestError);
        }, _manager.OnRequestError);
    }

    private IEnumerator ResetGuild()
    {
        EntityWithLineage admin = PlayerGuildMembers.Find(x => x.RoleId == "admins").Members[0];

        if (admin.Key.Id == _manager.Entity.Id)
        {
            yield return DeleteGuild();
            yield break;
        }

        EntityKey user = new()
        {
            Id = _manager.Entity.Id,
            Type = _manager.Entity.Type
        };

        yield return KickPlayer(user);
        OnInitComplete?.Invoke();
    }

    public IEnumerator CreateGuild(string name, string description)
    {
        //TODO -> Check if name already exists
        PlayerGuildData = new(description);

        yield return _manager.StartAsyncRequest("Creating guild...");
        _creatingGuild = true;

        CreateGroupResponse newGuild = null;

        PlayFabGroupsAPI.CreateGroup(new()
        {
            GroupName = name
        }, res =>
        {
            newGuild = res;

            PlayFabDataAPI.SetObjects(new()
            {
                Objects = new()
            {
                new()
                {
                    ObjectName = PlayerGuildData.GetType().Name,
                    DataObject = PlayerGuildData
                }
            },
                Entity = new() { Id = newGuild.Group.Id, Type = newGuild.Group.Type }
            }, res => AddFakePlayer(newGuild), _manager.OnRequestError);
        }, _manager.OnRequestError);
    }

    private void AddFakePlayer(CreateGroupResponse response)
    {
        PlayerGuild = response.Group;
        PlayerGuildName = response.GroupName;

        Debug.Log($"Guild {PlayerGuildName} created !");

        PlayFabGroupsAPI.ApplyToGroup(new() //Adding fake player to newly created guild
        {
            Entity = new() { Id = _adminEntity.Id, Type = _adminEntity.Type },
            Group = PlayerGuild
        }, res =>
        {
            PlayFabGroupsAPI.AcceptGroupApplication(new()
            {
                Entity = new() { Id = _adminEntity.Id, Type = _adminEntity.Type },
                Group = PlayerGuild
            }, res =>
            {
                _manager.EndRequest();
                Debug.Log($"Fake player added to Guild {PlayerGuildName} !");
                StartCoroutine(GetGuildData(PlayerGuild));

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
                    }, null, _manager.OnRequestError);
                }, _manager.OnRequestError);*/
            }, _manager.OnRequestError);
        }, _manager.OnRequestError);
    }

    public IEnumerator DeleteGuild()
    {
        yield return _manager.StartAsyncRequest("Deleting guild...");

        PlayFabGroupsAPI.DeleteGroup(new()
        {
            Group = PlayerGuild
        }, res => _manager.EndRequest("Guild deleted !"), _manager.OnRequestError);
    }

    public IEnumerator GetGuilds()
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListMembership(new()
        {
            Entity = new() { Id = _adminEntity.Id, Type = _adminEntity.Type }
        }, res => {
            _manager.InvokeOnGetGuilds(res.Groups);
            _manager.EndRequest();
        }, _manager.OnRequestError);
    }

    public IEnumerator GetPlayerOpportunities() //List of applications sent and invitations received by the player
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListMembershipOpportunities(new(), res =>
        {
            _manager.InvokeOnGetApplications(res.Applications);
            _manager.InvokeOnGetInvitations(res.Invitations);
            _manager.EndRequest();
        }, _manager.OnRequestError);
    }

    public IEnumerator ApplyToGuild(EntityKey guild)
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ApplyToGroup(new()
        {
            Group = guild
        }, res => _manager.EndRequest("Applied successfully !"), _manager.OnRequestError);
    }

    public IEnumerator GetGuildApplications()
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListGroupApplications(new()
        {
            Group = PlayerGuild
        }, res =>
        {
            _manager.InvokeOnGetApplications(res.Applications);
            _manager.EndRequest();
        }, _manager.OnRequestError);
    }

    public IEnumerator AcceptGuildApplication(string applicant) //String needed = ApplicantId#ApplicantType
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.AcceptGroupApplication(new()
        {
            Group = PlayerGuild,
            Entity = new() { Id = applicant.Split("#")[0], Type = applicant.Split("#")[1] }
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }

    public IEnumerator DenyGuildApplication(string applicant) //String needed = ApplicantId#ApplicantType
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.RemoveGroupApplication(new()
        {
            Group = PlayerGuild,
            Entity = new() { Id = applicant.Split("#")[0], Type = applicant.Split("#")[1] }
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }

    public IEnumerator SendGuildInvitation(string username)
    {
        yield return _manager.StartAsyncRequest();   

        PlayFabClientAPI.GetAccountInfo(new()
        {
            TitleDisplayName = username
        }, res =>
        {
            PlayFab.ClientModels.EntityKey entity = res.AccountInfo.TitleInfo.TitlePlayerAccount;

            PlayFabGroupsAPI.InviteToGroup(new()
            {
                Entity = new() { Id = entity.Id, Type = entity.Type },
                Group = PlayerGuild
            }, res => _manager.EndRequest("Invitation successfully sent !"), _manager.OnRequestError);
        }, _manager.OnRequestError);
    }

    public IEnumerator GetGuildInvitations() //List of invitations sent by a guild
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListGroupInvitations(new()
        {
            Group = PlayerGuild
        }, res =>
        {
            _manager.InvokeOnGetInvitations(res.Invitations);
            _manager.EndRequest();
        }, _manager.OnRequestError);
    }

    public IEnumerator AcceptGuildInvitation(string guild) //String needed = GuildId#GuildTyp
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.AcceptGroupInvitation(new()
        {
            Group = new()
            {
                Id = guild.Split("#")[0],
                Type = guild.Split("#")[1]
            }
        }, res => GetPlayerGuild(), _manager.OnRequestError);
    }

    public IEnumerator DenyGuildInvitation(string guild) //String needed = GuildId#GuildTyp
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.RemoveGroupInvitation(new()
        {
            Group = new()
            {
                Id = guild.Split("#")[0],
                Type = guild.Split("#")[1]
            }
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }

    public IEnumerator KickPlayer(EntityKey player)
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.RemoveMembers(new()
        {
            Group = PlayerGuild,
            Members = new() { player }
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }
}