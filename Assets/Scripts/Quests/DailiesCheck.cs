using System;
using UnityEngine;

public class DailiesCheck 
{
    //TODO -> Move to parent script
    //TODO -> Use only one action with param accross all quests
    public static event Action daily1;
    public static event Action daily2;
    public static event Action daily3;
    public static event Action daily4;
    public static event Action daily5;

    //TODO -> Move to parent script
    public int goldAmount;
    public int crystalsAmount;

    //TODO -> Move to parent script
    public int dungeonNeeded;
    public int mainStoryNeeded;
    public int gearUpgradeNeeded;
    public int enemyKilledNeeded;
    public int energyUsedNeeded;

    //TODO -> Move to parent script
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
            daily1?.Invoke();
        }
        if(_dungeonCount >= dungeonNeeded)
        {
            GiveRewards();
            daily2?.Invoke();
        }
    }

    private void CheckGearUpgrade()
    {
        if (_gearUpgradeCount >= gearUpgradeNeeded)
        {
            GiveRewards();
            daily3?.Invoke();
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
            daily4?.Invoke();
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
            daily5?.Invoke();
        }
        else
        {
            _energyUsedCount++;
        }
    }

    private void GiveRewards()
    {
        PlayFabManager.Instance.AddCurrency(PlayFabManager.GameCurrency.Gold, goldAmount);
        PlayFabManager.Instance.AddCurrency(PlayFabManager.GameCurrency.Crystals, goldAmount);
    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += CheckMission;
        Gear.LevelUp += CheckGearUpgrade;
        BattleSystem.OnEnemyKilled += CheckEnemyKilled;
        PlayFabManager.OnEnergyUsed += CheckEnergyUsed;
    }

    private void OnDisable()
    {
        MissionManager.OnMissionComplete -= CheckMission;
        Gear.LevelUp -= CheckGearUpgrade;
        BattleSystem.OnEnemyKilled -= CheckEnemyKilled;
        PlayFabManager.OnEnergyUsed -= CheckEnergyUsed;
    }
}