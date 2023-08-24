using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChapterScroll : MonoBehaviour
{
    public Scrollbar scrollbar;
    public RectTransform content;
    public Button[] chapterButtons;
    
    private float[] chapterPositions;

    private bool isDragging = false;

    private void Start()
    {
        CalculateChapterPositions();
        
        scrollbar.onValueChanged.AddListener(OnScroll);
        
        foreach (Button button in chapterButtons)
        {
            button.onClick.AddListener(() => JumpToChapter(button));
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void CalculateChapterPositions()
    {
        chapterPositions = new float[chapterButtons.Length];
        
        for (int i = 0; i < chapterButtons.Length; i++)
        {
            chapterPositions[i] = i / (float)(chapterButtons.Length - 1);
        }
    }
    
    private void OnScroll(float value)
    {
        Vector2 position = content.anchoredPosition;
        position.x = Mathf.Lerp(0, content.rect.width - scrollbar.GetComponent<RectTransform>().rect.width, value);
        content.anchoredPosition = position;
    }
    
    private void JumpToChapter(Button button)
    {
        int index = System.Array.IndexOf(chapterButtons, button);
        if (index >= 0)
        {
            float targetScrollValue = chapterPositions[index];
            scrollbar.value = targetScrollValue;
        }
    }
}
