using UnityEngine;
using UnityEngine.UI;

public class LoadBar : MonoBehaviour
{
    [SerializeField] private Slider _loadingSlider;

    private int _requestCount;
    private int _count;

    private void Awake()
    {
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
        _count = _requestCount;

        if (_requestCount >= 100)
        {
            _count = 100;
        }

        _loadingSlider.value = Mathf.Lerp(_loadingSlider.value, (_count + 10) / 100f, 0.01f);
    }

    private void RequestCalled()
    {
        _requestCount += 10;
    }
}