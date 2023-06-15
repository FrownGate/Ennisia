
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RaidsDifficulty : MonoBehaviour
{
    public MissionSO RaidDiff;


    private void OnMouseDown()
    {
        MissionManager.Instance.SetMission(RaidDiff);
    }
}