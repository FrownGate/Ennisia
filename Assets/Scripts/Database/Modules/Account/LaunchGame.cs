using NaughtyAttributes;
using UnityEngine;

public class LaunchGame : MonoBehaviour
{
    [SerializeField, Scene] private string _mainMenu;
    [SerializeField, Scene] private string _genderSelection;
    [Scene] private string _activeScene;

    private bool _canClick;

    private void Awake()
    {
        EnableClick();
        PlayFabManager.OnLoginSuccess += SetActiveScene;
        PlayFabManager.OnLoginError += EnableClick;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLoginSuccess -= SetActiveScene;
        PlayFabManager.OnLoginError -= EnableClick;
    }

    private void SetActiveScene()
    {
        _activeScene = PlayFabManager.Instance.Account.Gender == 0 ? _genderSelection : _mainMenu;
        ScenesManager.Instance.SetScene(_activeScene);
    }

    private void EnableClick()
    {
        _canClick = true;
    }

    private void OnMouseUpAsButton()
    {
        if (!_canClick) return;
        _canClick = false;

        if (!PlayFabManager.Instance.IsObsolete)
        {
            PlayFabManager.Instance.Login();
            return;
        }

        Application.Quit();
    }
}