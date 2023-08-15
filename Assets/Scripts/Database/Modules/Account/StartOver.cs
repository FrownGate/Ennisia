using TMPro;
using UnityEngine;

public class StartOver : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    //Prevent deleting dev testing account
    private bool IsDevTesting => !string.IsNullOrEmpty(_user) && _user == "testing@gmail.com";
    private string _user;

    private void Awake()
    {
        _user = null;
        gameObject.SetActive(false);
        PlayFabManager.OnLocalDatasChecked += Init;
    }

    private void OnDestroy()
    {
        PlayFabManager.OnLocalDatasChecked -= Init;
    }

    private void Init(string user)
    {
        _user = user;

        if (!string.IsNullOrEmpty(_user))
        {
            gameObject.SetActive(true);
        }

        if (IsDevTesting)
        {
            _text.text = "Dev Testing";
        }
    }

    private void OnMouseUpAsButton()
    {
        PlayFabManager.Instance.ResetAccount(IsDevTesting);
        //TODO -> add confirmation popup
    }
}