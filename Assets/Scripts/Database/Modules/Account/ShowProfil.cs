using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

public class ShowProfil : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _profilPrefab;

    private void Awake()
    {
        GameObject currentButton = Instantiate(_profilPrefab, _content.transform);

    }
}