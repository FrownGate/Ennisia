using UnityEngine;

public class SceneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        ScenesManager.Instance.SetScene(gameObject.name);
        //TODO -> add name to SceneManager
        //Add parent name if exist ?
    }
}