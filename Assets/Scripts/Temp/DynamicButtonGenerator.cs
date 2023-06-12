using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public int numberOfButtons = 10;

    private RectTransform contentRect;

    private void Start()
    {
        contentRect = GetComponent<ScrollRect>().content;
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        VerticalLayoutGroup layoutGroup = contentRect.GetComponentInChildren<VerticalLayoutGroup>();
        float buttonHeight = layoutGroup.preferredHeight;

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject button = Instantiate(buttonPrefab, contentRect);
            button.GetComponentInChildren<TMP_Text>().text = "Button " + (i + 1);
        }

        // Adjust the content size to fit all buttons
        float contentHeight = layoutGroup.preferredHeight;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
    }
}
