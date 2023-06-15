using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowGuilds : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _guildPrefab;

    private void Awake()
    {
        PlayFabManager.OnGetGuilds += InitGuilds;
        PlayFabManager.Instance.GetGuilds();
    }

    private void OnDestroy()
    {
        PlayFabManager.OnGetGuilds -= InitGuilds;
    }

    private void InitGuilds(List<GroupWithRoles> guilds)
    {
        foreach (GroupWithRoles guild in guilds)
        {
            GameObject guildObject = Instantiate(_guildPrefab, _content.transform);
        }
    }
}