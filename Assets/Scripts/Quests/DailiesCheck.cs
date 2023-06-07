using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DailiesCheck : MonoBehaviour
{
    public static event Action daily1;
    public static event Action daily2;
    public static event Action daily3;
    public static event Action daily4;

    public int goldAmount;
    public int crystalsAmount;

    public int dungeonNeeded;
    public int mainStoryNeeded;
    public int gearUpgradeNeeded;
    public int enemyKilledNeeded;
    public int energyUsedNeeded;
   
    int _dungeonCount;
    int _mainStoryCount;
    int _gearUpgradeCount;
    int _enemyKilledCount;
    int _energyUsedCount;


    private void CheckMission(MissionSO mission)
    {
        if (mission.Type == MissionManager.MissionType.Dungeon)
        {
            _dungeonCount++;
        }
        if (mission.Type == MissionManager.MissionType.MainStory)
        {
            _mainStoryCount++;
        }
        if (_mainStoryCount >= mainStoryNeeded || _dungeonCount >= dungeonNeeded)
        {
            GiveRewards();
        }
    }

    private void CheckGearUpgrade()
    {
        if (_gearUpgradeCount >= gearUpgradeNeeded)
        {
            GiveRewards();
        }
        else
        {
            _gearUpgradeCount++;
        }
    }


    private void CheckEnemyKilled()
    {
        if (_enemyKilledCount >= enemyKilledNeeded)
        {
            GiveRewards();
        }
        else
        {
            _enemyKilledCount++;
        }
    }

    private void CheckEnergyUsed()
    {
        if(_energyUsedCount >= energyUsedNeeded)
        {
            GiveRewards();
        }
        else
        {
            _energyUsedCount++;
        }
    }

    private void GiveRewards()
    {
        PlayFabManager.Instance.AddCurrency("Gold", goldAmount);
        PlayFabManager.Instance.AddCurrency("Crystals", goldAmount);
    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += CheckMission;
        Gear.LevelUp += CheckGearUpgrade;
        BattleSystem.enemyKilled += CheckEnemyKilled;
        PlayFabManager.OnEnergyUsed += CheckEnergyUsed;
    }

    private void OnDisable()
    {
        MissionManager.OnMissionComplete -= CheckMission;
        Gear.LevelUp -= CheckGearUpgrade;
        BattleSystem.enemyKilled -= CheckEnemyKilled;
        PlayFabManager.OnEnergyUsed -= CheckEnergyUsed;
    }
}