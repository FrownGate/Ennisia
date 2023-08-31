using NaughtyAttributes;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneButton : MonoBehaviour
{
    [Scene] public string Scene;
    [SerializeField] private string _params;
    public static event Action<string> PlaySFXOnSceneChange;
    public static event Action<string> PlayMusicOnSceneChange;

    private void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = GetComponent<RectTransform>().sizeDelta;
    }

    protected virtual void OnMouseUpAsButton()
    {
        //Debug.Log("SceneButton");
        if (gameObject.GetComponent<PLaySFX>() == null)
        {
            PlaySFXOnSceneChange?.Invoke(Scene);
        }
        PlayMusicOnSceneChange?.Invoke(Scene);
        ScenesManager.Instance.SetScene($"{Scene}#{_params}");
    }

    public void SetParam(string param)
    {
        _params = param;
    }
}