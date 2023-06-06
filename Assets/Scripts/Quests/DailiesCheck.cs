using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailiesCheck : MonoBehaviour
{
    public static event Action daily1;
    public static event Action daily2;
    public static event Action daily3;
    public static event Action daily4;
    public int dungeonNeeded;
    public int mainStoryNeeded;
    public int goldAmount;
    public int crystalsAmount;
    int _dungeonCount;
    int _mainStoryCount;


    private void Check(MissionSO mission)
    {
        if(mission.Type == MissionManager.MissionType.Dungeon)
        {
            _dungeonCount++;
        }
        if(mission.Type == MissionManager.MissionType.MainStory)
        {
            _mainStoryCount++;
        }
        if (_mainStoryCount >= mainStoryNeeded || _dungeonCount >= dungeonNeeded)
        {
            PlayFabManager.Instance.AddCurrency("Gold", goldAmount);
            PlayFabManager.Instance.AddCurrency("Crystals", goldAmount);
        }
    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += Check;
    }
}