using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionInfo : MonoBehaviour
{

    private void Start()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Enemies :";
        MissionSO mission = MissionManager.Instance.CurrentMission;
        foreach (string enemy in mission.Enemies)
        {
            buttonText.text += $"\n-{enemy}";
        }

    }
}
