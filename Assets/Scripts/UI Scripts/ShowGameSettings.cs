using System;
using TMPro;
using UnityEngine;

public class ShowGameSettings : MonoBehaviour
{
    [SerializeField] private GameObject _accountSettings;
    [SerializeField] private GameObject _gameSettings;

    public void OnMouseDown()
    {
        _accountSettings.SetActive(false);
        _gameSettings.SetActive(true);
    }
}