using NaughtyAttributes;
using UnityEngine;

public class RegistrationCheck : MonoBehaviour
{
    [SerializeField, Scene] private string _popup;
    
    private void Awake()
    {
        if (PlayFabManager.Instance.HasAuthData) return;
        ScenesManager.Instance.SetScene(_popup);
    }
}