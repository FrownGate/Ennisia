using UnityEngine;
using UnityEngine.UI;

public class ShowSupport : MonoBehaviour
{
    [SerializeField] private Image _image;

    private SupportCharacterSO _support;

    public void Init(SupportCharacterSO support)
    {
        _support = support;
    }
}