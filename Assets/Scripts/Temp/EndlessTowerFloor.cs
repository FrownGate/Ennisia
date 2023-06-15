using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTowerFloor : MonoBehaviour
{
    public MissionSO floorSO;

    private void OnMouseDown()
    {
        MissionManager.Instance.SetMission(floorSO);
    }
}
