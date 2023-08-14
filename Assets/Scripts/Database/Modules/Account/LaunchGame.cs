using NaughtyAttributes;
using UnityEngine;

public class LaunchGame : MonoBehaviour
{
    [SerializeField, Scene] private string _mainMenu;
    [SerializeField, Scene] private string _genderSelection;
    [Scene] private string _activeScene;

    private void Awake()
    {
        PlayFabManager.OnLoginSuccess += SetActiveScene;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLoginSuccess -= SetActiveScene;
    }

    private void SetActiveScene()
    {
        _activeScene = PlayFabManager.Instance.Account.Gender == 0 ? _genderSelection : _mainMenu;
        ScenesManager.Instance.SetScene(_activeScene);
    }

    private void OnMouseUpAsButton()
    {
        PlayFabManager.Instance.Login();
    }
}