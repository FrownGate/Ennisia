using UnityEngine;
using UnityEngine.UI;

public class PlayerGender : MonoBehaviour
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private Sprite _female;
    [SerializeField] private Sprite _male;

    void Start()
    {
        Image characterImage = _characterImage.GetComponent<Image>();
        switch (PlayFabManager.Instance.Account.Gender)
        {
            case 1:
                characterImage.sprite = _female;
                break;
            case 2:
                characterImage.sprite = _male;
                break;
        }
    }
}