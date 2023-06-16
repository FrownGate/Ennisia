using PlayFab.GroupsModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuildInfo : MonoBehaviour
{
    public static event Action<GroupWithRoles> OnClick;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _members;
    private GroupWithRoles _guild;

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
        _guild = guild;
        _name.text = _guild.GroupName;
        PlayFabManager.Instance.GetGuildMembers(_guild);
    }

    private void SetMembers(List<EntityMemberRole> members)
    {
        _members.text = $"{members.Count - 1}/30";
    }

    private void OnMouseUp()
    {
        OnClick?.Invoke(_guild);
    }
}