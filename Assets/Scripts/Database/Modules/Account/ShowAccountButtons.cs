using NaughtyAttributes;
using UnityEngine;

public class ShowAccountButtons : MonoBehaviour
{
    [SerializeField, Scene] private string _scene;
    [SerializeField] private GameObject _deleteAccountButton;
    [SerializeField] private GameObject _registerButton;

    private void Awake()
    {
        if (!PlayFabManager.Instance.HasAuthData)
        {
            _deleteAccountButton.SetActive(false);
            return;
        }

        _registerButton.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Register()
    {
        ScenesManager.Instance.SetScene(_scene);
    }
}