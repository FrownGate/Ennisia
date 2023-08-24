using TMPro;
using UnityEngine;

public class ShowAccountEmail : MonoBehaviour
{
    [SerializeField] private GameObject _parent;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        if (!PlayFabManager.Instance.HasAuthData)
        {
            _parent.SetActive(false);
            return;
        }

        _text.text = PlayFabManager.Instance.Data.Account.Email;
    }
}