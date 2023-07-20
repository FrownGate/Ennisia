using NaughtyAttributes;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneButton : MonoBehaviour
{
    [Scene] public string Scene;
    [SerializeField] private string _params;
    public static event Action<int> ChangeSceneSFX;

    private void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = GetComponent<RectTransform>().sizeDelta;
    }

    protected virtual void OnMouseUpAsButton()
    {
        ChangeSceneSFX?.Invoke(1);
        ScenesManager.Instance.SetScene($"{Scene}#{_params}");
    }

    public void SetParam(string param)
    {
        _params = param;
    }
}