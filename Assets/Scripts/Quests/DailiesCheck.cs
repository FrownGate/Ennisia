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
    int _dungeonCount;
    int _mainStoryCount;


    private void AddOne(MissionSO mission)
    {
        if(mission.Type == MissionManager.MissionType.Dungeon)
        {
            _dungeonCount++;
        }
        if(mission.Type == MissionManager.MissionType.MainStory)
        {
            _mainStoryCount++;
        }
        if (_mainStoryCount >= dungeonNeeded)
        {

        }
        if (_dungeonCount >= mainStoryNeeded)
        {

        }

    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += AddOne;
    }
}