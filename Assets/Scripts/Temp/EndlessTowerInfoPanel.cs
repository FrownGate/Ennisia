using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessTowerInfoPanel : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        MissionSO floorInfo = MissionManager.Instance.CurrentMission;

        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Enemies :";

        if (floorInfo!=null)
        {
            foreach (string enemy in floorInfo.Enemies)
            {
                buttonText.text += $"\n-{enemy}";
            }

        }
    }

}

