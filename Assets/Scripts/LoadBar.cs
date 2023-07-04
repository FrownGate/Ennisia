using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadBar : MonoBehaviour
{
    int _requestCount;
    int _count;
    public Slider _bar;
    Scene scene;
    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (scene.isLoaded)
        {
            _count = _requestCount;
            Debug.Log("i'm loading" + _count);

            if (_requestCount >= 99)
            {
                _count = 99;
            }
            _bar.value = Mathf.Lerp(_bar.value, _count/100f, 0.01f);

        }
    }

    private void requestCalled()
    {
        _requestCount += 10;
    }

    private void OnEnable()
    {
        PlayFabManager.OnRequest += requestCalled;
        PlayFabManager.OnEndRequest += requestCalled;
    }
    private void OnDisable()
    {
        PlayFabManager.OnRequest -= requestCalled;
        PlayFabManager.OnEndRequest -= requestCalled;
    }
}
