using NaughtyAttributes;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class SceneButton : MonoBehaviour
{
    [Scene]
    public string Scene;

    private void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = GetComponent<RectTransform>().sizeDelta;
    }
    private void OnMouseDown()
    {

        ScenesManager.Instance.SetScene(Scene); 
    }
}