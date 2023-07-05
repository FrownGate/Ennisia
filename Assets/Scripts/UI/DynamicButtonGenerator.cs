using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGenerator : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public float PaddingPercentage = 0.03f;

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

            RectTransform buttonRect = button.GetComponent<RectTransform>();
            

            // Get the original width and height of the button
            float originalWidth = buttonRect.sizeDelta.x;
            float originalHeight = buttonRect.sizeDelta.y;

            // Calculate the adjusted width based on the content width
            float adjustedWidth = _contentRect.rect.width * (1f - PaddingPercentage * 2f);
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
    public List<GameObject> AddButtonsToGridLayout(int numberOfButtons)
    {
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        ContentSizeFitter contentSizeFitter = GetComponent<ContentSizeFitter>();

        List<GameObject> buttonsCreated = new List<GameObject>();

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject button = Instantiate(ButtonPrefab, transform);
            buttonsCreated.Add(button);

            // Adjust the size of the button based on the cell size of the grid layout
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            float adjustedWidth = gridLayout.cellSize.x;
            float adjustedHeight = gridLayout.cellSize.y;
            buttonRect.sizeDelta = new Vector2(adjustedWidth, adjustedHeight);

            // Adjust the BoxCollider component if available
            BoxCollider2D boxCollider = button.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                boxCollider.size = new Vector2(adjustedWidth, adjustedHeight);
            }

            // Adjust the size of children objects within the button
            AdjustChildrenSize(button);
        }

        // Adjust the content size fitter
        contentSizeFitter.SetLayoutVertical();
        contentSizeFitter.SetLayoutHorizontal();

        return buttonsCreated;
    }

    private void AdjustChildrenSize(GameObject button)
    {
        // Get all the child objects within the button
        Transform[] children = button.GetComponentsInChildren<Transform>();

        // Adjust the size of each child object
        foreach (Transform child in children)
        {
            // Skip the button's own transform
            if (child == button.transform)
                continue;

            // Adjust the size of the child object if it has a RectTransform component
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                childRect.sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
            }

            // Adjust the BoxCollider component if available
            BoxCollider2D boxCollider = child.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                boxCollider.size = childRect.sizeDelta;
            }
        }
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