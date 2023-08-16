using UnityEngine;
using TMPro;

public class StartLoginRegister : MonoBehaviour
{
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_Text _button;

    private bool _loggedIn;

    private void Awake()
    {
        if (PlayFabManager.Instance.LoggedIn)
        {
            _loggedIn = true;
            _button.text = "Register";
            return;
        }

        _loggedIn = false;
    }

    public void Submit()
    {
        if (string.IsNullOrEmpty(_email.text) || string.IsNullOrEmpty(_password.text))
        {
            Debug.LogError("Email or password can't be empty.");
            return;
        }

        if (_loggedIn)
        {
            PlayFabManager.Instance.RegisterAccount(_email.text, _password.text);
            return;
        }

        PlayFabManager.Instance.Login(_email.text, _password.text);
    }
}