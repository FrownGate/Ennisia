using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        Debug.Log("Closing popup");
        ScenesManager.Instance.UnloadPopup(gameObject.scene);
    }
}