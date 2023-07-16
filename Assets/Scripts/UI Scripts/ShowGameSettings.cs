using UnityEngine;

public class ShowGameSettings : MonoBehaviour
{
    [SerializeField] private GameObject _accountSettings;
    [SerializeField] private GameObject _gameSettings;

    public void OnMouseUpAsButton()
    {
        _accountSettings.SetActive(false);
        _gameSettings.SetActive(true);
    }
}