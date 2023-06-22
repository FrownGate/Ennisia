using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGuild : MonoBehaviour
{
    [SerializeField] private Button _createButton;
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private TMP_InputField _description;

    public void SubmitCreate()
    {
        if (string.IsNullOrEmpty(_name.text) || string.IsNullOrEmpty(_description.text))
        {
            Debug.LogError("Guild name or description can't be empty.");
            return;
        }

        PlayFabManager.Instance.CreateGuild(_name.text, _description.text);
    }
}