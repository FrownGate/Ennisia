using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowMails : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _mailPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_mailPrefab, _content.transform);

    }
}