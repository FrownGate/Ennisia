using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMission : MonoBehaviour
{
    private void OnMouseDown()
    {
        MissionManager.Instance.ResetMissionManager();
    }
}
