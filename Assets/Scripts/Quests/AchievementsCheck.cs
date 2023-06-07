using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchievementsCheck : MonoBehaviour
{
    // COPY PASTE FROM DAILIES  


    public int goldAmount;
    public int crystalsAmount;

    public int dungeonNeeded;
    public int mainStoryNeeded;
    public int gearUpgradeNeeded;
    public int enemyKilledNeeded;
    public int energyUsedNeeded;

    //using events for every type of action in the game to manage the counts could be better

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
        if (_mainStoryCount >= mainStoryNeeded)
        {
            GiveRewards();
        }
        if (_dungeonCount >= dungeonNeeded)
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
        if (_energyUsedCount >= energyUsedNeeded)
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
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Gold, goldAmount);
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, goldAmount);
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
