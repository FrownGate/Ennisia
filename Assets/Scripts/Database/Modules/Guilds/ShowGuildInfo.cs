using PlayFab.GroupsModels;
using UnityEngine;

public class ShowGuildInfo : MonoBehaviour
{
    [SerializeField] private GameObject _applyButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    private EntityKey _guild;

    private void Awake()
    {
        GuildInfo.OnClick += ShowInfo;
    }

    private void OnDestroy()
    {
        GuildInfo.OnClick -= ShowInfo;
    }

    private void ShowInfo(EntityKey guild)
    {
        _canvasGroup.alpha = 1;
        _guild = guild;
        _applyButton.SetActive(_guild != PlayFabManager.Instance.PlayerGuild);
    }

    public void Apply()
    {
        PlayFabManager.Instance.ApplyToGuild(_guild);
    }
}