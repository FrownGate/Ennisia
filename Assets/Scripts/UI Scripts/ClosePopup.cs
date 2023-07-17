using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        Debug.Log("click");
        ScenesManager.Instance.UnloadPopup(gameObject.scene);
    }
}