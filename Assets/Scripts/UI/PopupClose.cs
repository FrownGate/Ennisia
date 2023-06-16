using UnityEngine;

public class PopupClose : MonoBehaviour
{
    public void OnMouseDown()
    {
        ScenesManager.Instance.ClosePopup();
    }
}
