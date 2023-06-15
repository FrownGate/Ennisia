using NaughtyAttributes;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    [Scene]
    public string Scene;
    private void OnMouseDown()
    {

        ScenesManager.Instance.SetScene(Scene); 
    }
}