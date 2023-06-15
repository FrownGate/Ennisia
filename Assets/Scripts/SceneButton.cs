using NaughtyAttributes;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    [Scene] public string Scene;
    [SerializeField] private string _params;

    private void OnMouseDown()
    {
        ScenesManager.Instance.SetScene($"{Scene}#{_params}"); 
    }
}