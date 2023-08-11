using TMPro;
using UnityEngine;

public class StartOver : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private bool IsDevTesting => !string.IsNullOrEmpty(_username) && _username == "testing5624e";
    private string _username;

    private void Awake()
    {
        DefaultDatas();
        PlayFabManager.OnLocalDatasChecked += Init;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLocalDatasChecked -= Init;
    }

    private void DefaultDatas()
    {
        _username = null;
        gameObject.SetActive(false);
    }

    private void Init(string username)
    {
        _username = username;

        if (!string.IsNullOrEmpty(_username))
        {
            gameObject.SetActive(true);
        }

        if (IsDevTesting)
        {
            _text.text = "Anonymous Login";
        }
    }

    private void OnMouseUpAsButton()
    {
        if (IsDevTesting) //Prevent deleting dev testing account
        {
            PlayFabManager.Instance.DevAnonymousLogin();
            return;
        }

        //TODO -> add confirmation popup + reset account

        DefaultDatas();
    }
}