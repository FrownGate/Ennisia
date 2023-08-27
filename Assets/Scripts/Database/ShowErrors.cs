using PlayFab;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShowErrors : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _textBackground;

    private void Awake()
    {
        PlayFabManager.OnError += Show;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnError -= Show;
    }

    private void Show(PlayFabError error)
    {
        _text.text = error.ErrorMessage;
        _text.gameObject.SetActive(true);
        _textBackground.SetActive(true);

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        _text.text = "";
        _text.gameObject.SetActive(false);
        _textBackground.SetActive(false);
    }
}