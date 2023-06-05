using UnityEngine;

public class SceneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        ScenesManager.Instance.SetScene(gameObject.name);
    }
}