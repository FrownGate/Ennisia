using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowGuildShopItem : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _guildShopItemPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_guildShopItemPrefab, _content.transform);

    }
}