using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGenerator : MonoBehaviour
{
    public GameObject ButtonPrefab;

    private RectTransform _contentRect;
    private VerticalLayoutGroup _layoutGroup;
    public float paddingPercentage = 0.03f;
    public List<GameObject> GenerateButtonsInSlider(int numberOfButtons)
    {
        List<GameObject> buttonsCreated = new();
        _contentRect = GetComponent<ScrollRect>().content;
        _layoutGroup = _contentRect.GetComponentInChildren<VerticalLayoutGroup>();

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject button = Instantiate(ButtonPrefab, _contentRect);
            buttonsCreated.Add(button);

            RectTransform buttonRect = button.GetComponent<RectTransform>();
            

            // Get the original width and height of the button
            float originalWidth = buttonRect.sizeDelta.x;
            float originalHeight = buttonRect.sizeDelta.y;

            // Calculate the adjusted width based on the content width
            float adjustedWidth = _contentRect.rect.width * (1f - paddingPercentage * 2f);
            float ratio = adjustedWidth / originalWidth;
            float adjustedHeight = originalHeight * ratio;

            // Set the adjusted size
            buttonRect.sizeDelta = new Vector2(adjustedWidth, adjustedHeight);

            // Adjust the BoxCollider component
            BoxCollider2D boxCollider = button.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(adjustedWidth, adjustedHeight);
            }
        }

        // Adjust the content size to fit all buttons
        float totalHeight = CalculateTotalHeight(buttonsCreated);
        _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, totalHeight);

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