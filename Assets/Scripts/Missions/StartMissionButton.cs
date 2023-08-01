using UnityEngine;

public class StartMissionButton : MonoBehaviour
{
    public void OnMouseUpAsButton()
    {
        MissionManager.Instance.StartMission();
    }
}