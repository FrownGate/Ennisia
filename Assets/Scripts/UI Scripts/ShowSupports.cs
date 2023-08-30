using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSupports : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _supportBannerPrefab;

    private void Awake()
    {
        RectTransform rect = _container.GetComponent<RectTransform>();

        foreach (string rarity in Enum.GetNames(typeof(Rarity)).Reverse()) {
            SupportCharacterSO[] supports = Resources.LoadAll<SupportCharacterSO>($"SO/SupportsCharacter/{rarity}");

            foreach (SupportCharacterSO character in supports)
            {
                var support = Instantiate(_supportBannerPrefab, _container.transform);
                support.GetComponentInChildren<TMP_Text>().text = character.Name;
                float y = rect.sizeDelta.y + support.GetComponent<RectTransform>().rect.height + _container.GetComponent<VerticalLayoutGroup>().spacing;
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
            }
        }
    }
}