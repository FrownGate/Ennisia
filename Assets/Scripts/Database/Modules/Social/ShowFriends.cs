using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowFriends : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _friendPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_friendPrefab, _content.transform);

    }
}