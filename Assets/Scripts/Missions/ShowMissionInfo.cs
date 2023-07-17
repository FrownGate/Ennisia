using TMPro;
using UnityEngine;

public class ShowMissionInfo : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Enemies :";
        MissionSO mission = MissionManager.Instance.CurrentMission;

        foreach (string enemy in mission.Enemies) buttonText.text += $"\n-{enemy}";
    }
}