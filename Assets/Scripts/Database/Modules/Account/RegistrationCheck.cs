using NaughtyAttributes;
using UnityEngine;

public class RegistrationCheck : MonoBehaviour
{
    [SerializeField, Scene] private string _popup;
    
    private void Awake()
    {
        if (PlayFabManager.Instance.HasAuthData) return;
        if (PlayFabManager.Instance.AccountChecked) return;
        PlayFabManager.Instance.AccountChecked = true;
        ScenesManager.Instance.SetScene(_popup);
    }
}