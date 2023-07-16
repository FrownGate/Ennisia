using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowGuilds : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _guildPrefab;

    private void Start()
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
        int position = 0;

        //TODO -> get all guild datas here with coroutine then use alpha visibility

        foreach (GroupWithRoles guild in guilds)
        {
            Debug.Log(guild.GroupName);
            GameObject guildObject = Instantiate(_guildPrefab, _content.transform);
            guildObject.transform.localPosition += new Vector3(0, position, 0);
            guildObject.GetComponent<GuildInfo>().Init(guild);
            position -= 110;
        }
    }
}