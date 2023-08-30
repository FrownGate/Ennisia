using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSupports : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _supportBannerPrefab;
    [SerializeField] private Image _supportImage;
    [SerializeField] private TMP_Text _supportText;

    private void Awake()
    {
        ShowSupportData.OnClick += ShowData;

        RectTransform rect = _container.GetComponent<RectTransform>();

        foreach (string rarity in Enum.GetNames(typeof(Rarity)).Reverse()) {
            SupportCharacterSO[] supports = Resources.LoadAll<SupportCharacterSO>($"SO/SupportsCharacter/{rarity}");

            foreach (SupportCharacterSO character in supports)
            {
                var support = Instantiate(_supportBannerPrefab, _container.transform);
                support.GetComponent<ShowSupportData>().Support = character;
                support.GetComponentInChildren<TMP_Text>().text = character.Name;
                float y = rect.sizeDelta.y + support.GetComponent<RectTransform>().rect.height + _container.GetComponent<VerticalLayoutGroup>().spacing;
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
            }
        }
    }

    private void OnDestroy()
    {
        ShowSupportData.OnClick -= ShowData;
    }

    private void ShowData(SupportCharacterSO support)
    {
        Sprite sprite = Resources.Load<Sprite>($"Textures/Supports/{support.Name}_art") ?? Resources.Load<Sprite>("Textures/Supports/empty");
        Debug.Log(sprite.name);
        _supportImage.sprite = sprite;
        _supportText.text = $"{support.Description}\n\n";

        support.Init();

        foreach (var skill in support.Skills)
        {
            _supportText.text += $"<b>{skill.Data.Name}</b>\n{skill.Data.Description}\n\n";
        }
    }
}