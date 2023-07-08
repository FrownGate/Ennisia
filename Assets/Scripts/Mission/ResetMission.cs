using UnityEngine;

public class ResetMission : MonoBehaviour
{
    private void OnMouseDown()
    {
        MissionManager.Instance.ResetData();
    }
}