using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateChapterName : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = MissionManager.Instance.CurrentChapter.Name;
    }
}