using System.Collections;
using TMPro;
using UnityEngine;

public class ShowWhenLocalDatasChecked : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private Animation _startAnimation;
    [SerializeField] private AnimationClip _fadeIn;
    [SerializeField] private AnimationClip _fadeOut;

    private void Awake()
    {
        gameObject.SetActive(false);
        PlayFabManager.OnLocalDatasChecked += ShowUI;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLocalDatasChecked -= ShowUI;
    }

    private void ShowUI(string user)
    {
        gameObject.SetActive(true);

        _startText.text = !string.IsNullOrEmpty(user) ?
            $"Registered account found. Click anywhere to login to {user}."
            : PlayFabManager.Instance.IsFirstLogin ?
            "No local data found. Click anywhere to create a new account."
            : "Local data found. Click anywhere to continue.";

        StartCoroutine(FadeAnimation());
    }

    private IEnumerator FadeAnimation()
    {
        _startAnimation.Play(_fadeIn.name);
        yield return new WaitForSeconds(_fadeIn.length * 2);
        _startAnimation.Play(_fadeOut.name);
        yield return new WaitForSeconds(_fadeOut.length);
        StartCoroutine(FadeAnimation());
    }
}