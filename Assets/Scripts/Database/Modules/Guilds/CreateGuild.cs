using TMPro;
using UnityEngine;

public class CreateGuild : MonoBehaviour
{
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private TMP_InputField _description;

    public void SubmitCreate()
    {
        if (string.IsNullOrEmpty(_name.text) || string.IsNullOrEmpty(_description.text))
        {
            Debug.LogError("Guild name or description can't be empty.");
            return;
        }

        if (!PlayFabManager.Instance.HasEnoughCurrency(100000, Currency.Gold)) return;
        PlayFabManager.Instance.CreateGuild(_name.text, _description.text);
    }
}