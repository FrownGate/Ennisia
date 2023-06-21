using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowEvents : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _eventPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_eventPrefab, _content.transform);

    }
}