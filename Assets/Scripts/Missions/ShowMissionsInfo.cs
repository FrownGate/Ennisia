using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowMissionsInfo : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _guildMissionInfoPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_guildMissionInfoPrefab, _content.transform);

    }
}