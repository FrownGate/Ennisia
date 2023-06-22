using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowQuests : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _questPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_questPrefab, _content.transform);

    }
}