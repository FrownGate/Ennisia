using TMPro;
using UnityEngine;

public class ShowAccountName : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.text = PlayFabManager.Instance.Data.Account.Name;
    }
}