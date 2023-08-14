using TMPro;
using UnityEngine;

public class ShowWhenLocalDatasChecked : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _startText;

    private void Awake()
    {
        gameObject.SetActive(false);
        PlayFabManager.OnLocalDatasChecked += ShowUI;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLocalDatasChecked -= ShowUI;
    }

    private void ShowUI(string user)
    {
        gameObject.SetActive(true);

        _startText.text = !string.IsNullOrEmpty(user) ?
            $"Local data found. Click anywhere to login to {user}."
            : "No local data found. Click anywhere to create a new account.";
    }
}