using TMPro;
using UnityEngine;

public class ShowUsername : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        PlayFabManager.OnLoginSuccess += SetUsername;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLoginSuccess -= SetUsername;
    }

    private void SetUsername()
    {
        _text.text = $"Logged in as {PlayFabManager.Instance.Account.Name}";
    }
}
