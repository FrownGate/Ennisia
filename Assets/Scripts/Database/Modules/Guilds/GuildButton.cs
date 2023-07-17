using NaughtyAttributes;
using UnityEngine;

public class GuildButton : MonoBehaviour
{
    [SerializeField, Scene] private string _playerGuild;
    [SerializeField, Scene] private string _guildsList;
    [Scene] private string _activeScene;

    private void Awake()
    {
        _activeScene = PlayFabManager.Instance.PlayerGuild != null ? _playerGuild : _guildsList;
    }
    private void OnMouseDown()
    {
        ScenesManager.Instance.SetScene(_activeScene);
    }
}