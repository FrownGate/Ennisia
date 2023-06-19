using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGenerator : MonoBehaviour
{
    public GameObject ButtonPrefab;

    private RectTransform _contentRect;
    private VerticalLayoutGroup _layoutGroup;


    public List<GameObject> GenerateButtonsInSlider(int numberOfButtons)
    {
        List<GameObject> buttonsCreated = new();
        _contentRect = GetComponent<ScrollRect>().content;
        _layoutGroup = _contentRect.GetComponentInChildren<VerticalLayoutGroup>();
        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject button = Instantiate(ButtonPrefab, _contentRect);
            buttonsCreated.Add(button);
        }

        // Adjust the content size to fit all buttons
        float totalHeight = CalculateTotalHeight(buttonsCreated);
        _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, totalHeight);
      //  GetComponentInChildren<Scrollbar>().value = 1;
        return buttonsCreated;
    }

    private float CalculateTotalHeight(List<GameObject> buttons)
    {
        float totalHeight = 0f;
        int childCount = buttons.Count;

        for (int i = 0; i < childCount; i++)
        {
            RectTransform childRect = buttons[i].GetComponent<RectTransform>();
            totalHeight += childRect.sizeDelta.y + _layoutGroup.spacing;
        }

        totalHeight += _layoutGroup.padding.top + _layoutGroup.padding.bottom;
        return totalHeight;
    }
}
