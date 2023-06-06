using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekliesCheck : MonoBehaviour
{
    public static event Action weekly1;
    public static event Action weekly2;
    public static event Action weekly3;
    public static event Action weekly4;
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