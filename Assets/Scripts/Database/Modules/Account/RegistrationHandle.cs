using NaughtyAttributes;
using UnityEngine;

public class RegistrationHandle : MonoBehaviour
{
    [SerializeField, Scene] private string _scene;

    public void StartRegister()
    {
        ScenesManager.Instance.SetScene(_scene);
    }

    public void Close()
    {
        ScenesManager.Instance.UnloadPopup(gameObject.scene);
    }
}