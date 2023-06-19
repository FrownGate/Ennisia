using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessTowerInfoPanel : MonoBehaviour
{
    private GameObject _startBtn;

    public GameObject PrefabStart;
    // Update is called once per frame
    void Update()
    {
        MissionSO floorInfo = MissionManager.Instance.CurrentMission;

        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Enemies :";

        if (floorInfo != null)
        {
            if (_startBtn == null)
            {

                _startBtn = Instantiate(PrefabStart, transform);
                Vector2 rectSize = GetComponent<RectTransform>().sizeDelta;
                Vector2 rectPos = transform.position;
                Vector2 pos = new(rectPos.x + (rectSize.x / 2 - _startBtn.GetComponent<RectTransform>().sizeDelta.x/2), rectPos.y - (rectSize.y / 2 - _startBtn.GetComponent<RectTransform>().sizeDelta.x / 2));
                _startBtn.transform.localPosition = pos;
            }
            foreach (string enemy in floorInfo.Enemies)
            {
                buttonText.text += $"\n-{enemy}";
            }

        }
    }

}

