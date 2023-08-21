using PlayFab.GroupsModels;
using TMPro;
using UnityEngine;

public class ShowGuildInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private GameObject _applyButton;
    [SerializeField] private CanvasGroup _canvasGroup;

    private GroupWithRoles _guild;

    private void Awake()
    {
        GuildInfo.OnClick += ShowInfo;
    }

    private void OnDestroy()
    {
        GuildInfo.OnClick -= ShowInfo;
    }

    private void ShowInfo(GroupWithRoles guild, GuildData data)
    {
        _canvasGroup.alpha = 1;
        _guild = guild;
        _name.text = guild.GroupName;
        _description.text = data.Description;
        _applyButton.SetActive(_guild.Group != PlayFabManager.Instance.PlayerGuild);
    }

    public void Apply()
    {
        PlayFabManager.Instance.ApplyToGuild(_guild.Group);
    }
}