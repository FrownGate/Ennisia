using TMPro;
using UnityEngine;

public class ShowAccountLevel : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.text = $"LVL {PlayFabManager.Instance.Data.Account.Level}";
    }
}