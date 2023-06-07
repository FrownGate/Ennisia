using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateChapterName : MonoBehaviour
{
    Text text;
    private void Start()
    {
        text = GetComponent<Text>();
        text.text = MissionManager.Instance.CurrentChapter.Name;
    }
}
