using NaughtyAttributes;
using UnityEngine;

public class LaunchGame : MonoBehaviour
{
    [SerializeField, Scene] private string _mainMenu;
    [SerializeField, Scene] private string _genderSelection;
    [Scene] private string _activeScene;

    private void Awake()
    {
        _activeScene = PlayFabManager.Instance.Account.Gender == 0 ? _genderSelection : _mainMenu;
    }

    private void OnMouseDown()
    {
        ScenesManager.Instance.SetScene(_activeScene);
    }
}