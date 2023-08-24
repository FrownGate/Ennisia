using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTitleScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject _buttons;
    [SerializeField] private Image _textBackground;
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private Animation _startAnimation;
    [SerializeField] private AnimationClip _fadeIn;
    [SerializeField] private AnimationClip _fadeOut;

    private void Awake()
    {
        gameObject.SetActive(false);
        PlayFabManager.OnLocalDatasChecked += ShowUI;
        PlayFabManager.OnObsoleteVersion += ShowUI;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLocalDatasChecked -= ShowUI;
        PlayFabManager.OnObsoleteVersion -= ShowUI;
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
        if (!_startAnimation.isPlaying) StartCoroutine(FadeAnimation());

        if (!PlayFabManager.Instance.IsObsolete) return;
        _buttons.SetActive(false);
        _startText.text = "Your game version isn't up to date ! Click anywhere to close the game.";
        _textBackground.color = new Color(1, 0, 0, 0.8f);
    }

    private void ShowUI(string user)
    {
        ShowUI();

        _startText.text = !string.IsNullOrEmpty(user) ?
            $"Registered account found. Click anywhere to login to {user}."
            : PlayFabManager.Instance.HasAuthFile ?
            "Local data found. Click anywhere to continue."
            : "No local data found. Click anywhere to create a new account.";
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