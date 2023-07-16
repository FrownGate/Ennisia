using System;
using UnityEngine;

public class ShowAccountSettings : MonoBehaviour
{
    public static event Action<string> OnPetClick;

    [SerializeField] private GameObject _accountSettings;
    [SerializeField] private GameObject _gameSettings;

    public void OnMouseDown()
    {
        _accountSettings.SetActive(true);
        _gameSettings.SetActive(false);
    }
}