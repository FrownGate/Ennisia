using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadBar : MonoBehaviour
{
    public Slider Bar;

    private int _requestCount;
    private int _count;
    private Scene _scene;

    private void Awake()
    {
        _scene = SceneManager.GetActiveScene();
        PlayFabManager.OnRequest += RequestCalled;
        PlayFabManager.OnEndRequest += RequestCalled;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnRequest -= RequestCalled;
        PlayFabManager.OnEndRequest -= RequestCalled;
    }

    private void Update()
    {
        if (_scene.isLoaded)
        {
            _count = _requestCount;
            Debug.Log("i'm loading" + _count);

            if (_requestCount >= 99)
            {
                _count = 99;
            }

            Bar.value = Mathf.Lerp(Bar.value, _count/100f, 0.01f);
        }
    }

    private void RequestCalled()
    {
        _requestCount += 10;
    }
}