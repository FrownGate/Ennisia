using UnityEngine;
using TMPro;

public class StartLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;

    public void SubmitLogin()
    {
        if (string.IsNullOrEmpty(_email.text) || string.IsNullOrEmpty(_password.text))
        {
            Debug.LogError("Email or password can't be empty.");
            return;
        }

        PlayFabManager.Instance.Login(_email.text, _password.text);
    }
}