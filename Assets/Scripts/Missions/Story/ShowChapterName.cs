using TMPro;
using UnityEngine;

public class ShowChapterName : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = MissionManager.Instance.CurrentChapter.Name;
    }
}