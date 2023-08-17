using PlayFab.GroupsModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuildInfo : MonoBehaviour
{
    public static event Action<EntityKey> OnClick;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _members;
    private GroupWithRoles _guild;

    private void Awake()
    {
        PlayFabManager.OnGetGuildData += SetData;
    }

    public void Init(GroupWithRoles guild)
    {
        _guild = guild;
        _name.text = _guild.GroupName;
        PlayFabManager.Instance.GetGuildData(_guild.Group);
    }

    private void SetData(GuildData data, List<EntityMemberRole> members)
    {
        //TODO -> add guild description
        Debug.Log($"Found {members.Count} member(s).");
        _members.text = $"{members.Count - 1}/30";
        PlayFabManager.OnGetGuildData -= SetData;
    }

    private void OnMouseUp()
    {
        OnClick?.Invoke(_guild.Group);
    }
}