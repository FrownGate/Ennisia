using PlayFab.GroupsModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowGuilds : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _guildPrefab;
    [SerializeField] private TMP_Text _emptyText;

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
        //TODO -> use vertical layout instead

        if (guilds.Count == 0) _emptyText.gameObject.SetActive(true);

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