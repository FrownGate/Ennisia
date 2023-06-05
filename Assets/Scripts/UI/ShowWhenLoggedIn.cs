using UnityEngine;

public class ShowWhenLoggedIn : MonoBehaviour
{
    private void Start()
    {
        ShowUI();
        PlayFabManager.OnLoginSuccess += ShowUI;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLoginSuccess -= ShowUI;
    }

    private void ShowUI()
    {
        gameObject.SetActive(PlayFabManager.Instance.LoggedIn);
    }
}
