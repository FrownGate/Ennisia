using PlayFab.GroupsModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuildInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _members;

    private void Awake()
    {
        PlayFabManager.OnGetGuildMembers += SetMembers;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnGetGuildMembers -= SetMembers;
    }

    public void Init(GroupWithRoles guild)
    {
        _name.text = guild.GroupName;
        PlayFabManager.Instance.GetGuildMembers(guild);
    }

    private void SetMembers(List<EntityMemberRole> members)
    {
        _members.text = $"{members.Count - 1}/30";
    }
}