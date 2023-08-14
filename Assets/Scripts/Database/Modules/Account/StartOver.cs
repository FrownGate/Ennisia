using TMPro;
using UnityEngine;

public class StartOver : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    //Prevent deleting dev testing account
    private bool IsDevTesting => !string.IsNullOrEmpty(_username) && _username == "testing5624e";
    private string _username;

    private void Awake()
    {
        _username = null;
        gameObject.SetActive(false);
        PlayFabManager.OnLocalDatasChecked += Init;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLocalDatasChecked -= Init;
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
        PlayFabManager.Instance.ResetAccount(IsDevTesting);
        //TODO -> add confirmation popup
    }
}