using UnityEngine;

public class ResetMission : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        MissionManager.Instance.ResetData();
    }
}