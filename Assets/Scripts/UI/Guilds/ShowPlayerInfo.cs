using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerInfo : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _playerInfoPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_playerInfoPrefab, _content.transform);

    }
}