using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadBar : MonoBehaviour
{
    int _requestCount;
    int _count;

    private Slider _barPC;
    private Slider _barMobile;

    Scene scene;
    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
#if UNITY_STANDALONE
        _barPC = GameObject.Find("PC Canvas").GetComponentInChildren<Slider>();
#endif
#if UNITY_IOS || UNITY_ANDROID
        BarMobile = GameObject.Find("Mobile Canvas").GetComponentInChildren<Slider>();
#endif
    }

    private void Update()
    {
        if (scene.isLoaded)
        {
            _count = _requestCount;
            if (_requestCount >= 100)
            {
                _count = 100;
            }
#if UNITY_STANDALONE
            _barPC.value = Mathf.Lerp(_barPC.value, (_count+10) / 100f, 0.01f);
#endif
#if UNITY_IOS || UNITY_ANDROID
            _barMobile.value = Mathf.Lerp(BarPC.value, _count / 100f, 0.01f);
#endif
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
