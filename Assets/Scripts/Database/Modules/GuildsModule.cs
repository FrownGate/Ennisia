using PlayFab;
using PlayFab.GroupsModels;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class GuildsModule : Module
{
    public static event Action OnInitComplete;
    public GroupWithRoles PlayerGuild { get; private set; }
    public GuildData PlayerGuildData { get; private set; }
    public List<EntityMemberRole> PlayerGuildMembers { get; private set; }

    private EntityKey _guildEntity; //Current request's guild's entity
    private PlayFab.ClientModels.EntityKey _adminEntity;
    //private readonly string _fakeRole = "Fake"; //Used ?

    //TODO -> Remove guild member
    //TODO -> check role for applications functions
    //TODO -> Check if player's application has been accepted in update ?

    private void Awake()
    {
        _adminEntity = new()
        {
            Id = "2641E2E5FB9FA5C2",
            Type = "title_player_account"
        }; //TODO -> encrypt datas
           //Can be replace by Get Account Info request
    }

    public void GetPlayerGuild()
    {
        _manager.StartRequest("Fetching player's guild...");

        PlayFabGroupsAPI.ListMembership(new(), res =>
        {
            _manager.EndRequest();
            PlayerGuild = res.Groups.Count > 0 ? res.Groups[0] : null;

            if (PlayerGuild != null)
            {
                Debug.Log($"Player is a member of Guild {PlayerGuild.GroupName}.");
                StartCoroutine(GetGuildData(PlayerGuild));
                return;
            }

            if (!_manager.LoggedIn) OnInitComplete?.Invoke();

        }, _manager.OnRequestError);
    }

    public IEnumerator GetGuildData(GroupWithRoles guild)
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListGroupMembers(new()
        {
            Group = guild.Group
        }, res =>
        {
            PlayerGuildMembers = guild == PlayerGuild ? res.Members : PlayerGuildMembers;

            if (guild == PlayerGuild && PlayerGuildData != null)
            {
                StartCoroutine(UpdatePlayerGuild());
                return;
            }

            PlayFabDataAPI.GetObjects(new()
            {
                Entity = new() { Id = guild.Group.Id, Type = guild.Group.Type },
                EscapeObject = true
            }, res =>
            {
                GuildData data = new(res.Objects[new GuildData().GetType().Name]);
                PlayerGuildData = guild == PlayerGuild ? data : PlayerGuildData;

                PlayFabGroupsAPI.ListGroupMembers(new()
                {
                    Group = guild.Group
                }, res =>
                {
                    _manager.InvokeOnGetGuildData(data, res.Members);
                    if (!_manager.LoggedIn) OnInitComplete?.Invoke();
                }, _manager.OnRequestError);
            }, _manager.OnRequestError);
        }, _manager.OnRequestError);
    }

    public IEnumerator CreateGuild(string name, string description)
    {
        Debug.Log("Creating guild...");
        //TODO -> save description as object

        //TODO -> Check if name already exists
        PlayerGuildData = new(description);

        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.CreateGroup(new()
        {
            GroupName = name
        }, OnCreateGuild, _manager.OnRequestError);
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
                //TODO -> load guild scene

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

    public IEnumerator UpdatePlayerGuild()
    {
        yield return _manager.StartAsyncRequest();

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
            Entity = new() { Id = PlayerGuild.Group.Id, Type = PlayerGuild.Group.Type }
        }, res => _manager.EndRequest(), _manager.OnRequestError);
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

    public IEnumerator ApplyToGuild(GroupWithRoles guild)
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ApplyToGroup(new()
        {
            Group = guild.Group
        }, res => _manager.EndRequest("Applied successfully !"), _manager.OnRequestError);
    }

    public IEnumerator GetGuildApplications()
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListGroupApplications(new()
        {
            Group = PlayerGuild.Group
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
            Group = PlayerGuild.Group,
            Entity = new() { Id = applicant.Split("#")[0], Type = applicant.Split("#")[1] }
        }, res => _manager.EndRequest(), _manager.OnRequestError);
    }

    public IEnumerator DenyGuildApplication(string applicant) //String needed = ApplicantId#ApplicantType
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.RemoveGroupApplication(new()
        {
            Group = PlayerGuild.Group,
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
                Group = PlayerGuild.Group
            }, res => _manager.EndRequest("Invitation successfully sent !"), _manager.OnRequestError);
        }, _manager.OnRequestError);
    }

    public IEnumerator GetGuildInvitations() //List of invitations sent by a guild
    {
        yield return _manager.StartAsyncRequest();

        PlayFabGroupsAPI.ListGroupInvitations(new()
        {
            Group = PlayerGuild.Group
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
}