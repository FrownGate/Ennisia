using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSupport : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    private SupportCharacterSO _support;

    public void Init(SupportCharacterSO support)
    {
        _support = support;
        _text.text = _support.Name;
        _text.color = PlayFabManager.Instance.RarityColors[support.Rarity];
    }
}