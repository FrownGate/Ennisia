using UnityEngine;

public class ShowAccountSettings : MonoBehaviour
{
    [SerializeField] private GameObject _accountSettings;
    [SerializeField] private GameObject _gameSettings;

    public void OnMouseUpAsButton()
    {
        _accountSettings.SetActive(true);
        _gameSettings.SetActive(false);
    }
}