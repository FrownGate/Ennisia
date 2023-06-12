using UnityEngine;
using UnityEngine.UI;

public class UpdateChapterName : MonoBehaviour
{
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        _text.text = MissionManager.Instance.CurrentChapter.Name;
    }
}