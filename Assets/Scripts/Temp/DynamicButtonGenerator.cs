using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;

    private RectTransform contentRect;
    private VerticalLayoutGroup layoutGroup;


    public List<GameObject> GenerateButtonsInSlider(int numberOfButtons)
    {
        List<GameObject> buttonsCreated = new();
        contentRect = GetComponent<ScrollRect>().content;
        layoutGroup = contentRect.GetComponentInChildren<VerticalLayoutGroup>();
        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject button = Instantiate(buttonPrefab, contentRect);
            buttonsCreated.Add(button);
        }

        // Adjust the content size to fit all buttons
        float totalHeight = CalculateTotalHeight(buttonsCreated);
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
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
            totalHeight += childRect.sizeDelta.y + layoutGroup.spacing;
        }

        totalHeight += layoutGroup.padding.top + layoutGroup.padding.bottom;
        return totalHeight;
    }
}
