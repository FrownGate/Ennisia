using System;
using UnityEngine;

public class WeekliesCheck 
{
    //TODO -> Move to parent script
    //TODO -> Use only one action with param accross all quests
    public static event Action weekly1;
    public static event Action weekly2;
    public static event Action weekly3;
    public static event Action weekly4;

    //TODO -> Move to parent script
    public int goldAmount;
    public int crystalsAmount;

    //TODO -> Move to parent script
    public int mainStoryNeeded;
    public int raidNeeded;
    public int expeditionNeeded;
    public int enemyKilledNeeded;

    //TODO -> Move to parent script
    int _mainStoryCount;
    int _raidCount;
    int _expeditionCount;
    int _enemyKilledCount;

    private void CheckMission(MissionSO mission)
    {
        if (mission.Type == MissionType.MainStory)
        {
            _mainStoryCount++;
        }
        if (mission.Type == MissionType.Raid)
        {
            _raidCount++;
        }
        if (mission.Type == MissionType.Expedition)
        {
            _expeditionCount++;
        }

        //add tower condition : Do 10 floor in the endless tower

        if (_mainStoryCount >= mainStoryNeeded)
        {
            GiveRewards();
            weekly1?.Invoke();
        }
        if(_raidCount >= raidNeeded)
        {
            GiveRewards();
            weekly2?.Invoke();
        }
        if (_expeditionCount >= expeditionNeeded)
        {
            GiveRewards();
            weekly3?.Invoke();
        }
    }



    private void CheckEnemyKilled(string name)
    {
        if (_enemyKilledCount >= enemyKilledNeeded)
        {
            GiveRewards();
            weekly4?.Invoke();
        }
        else
        {
            _enemyKilledCount++;
        }
    }


    private void GiveRewards()
    {
        PlayFabManager.Instance.AddCurrency(Currency.Gold, goldAmount);
        PlayFabManager.Instance.AddCurrency(Currency.Crystals, goldAmount);
    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += CheckMission;
        BattleSystem.OnEnemyKilled += CheckEnemyKilled;
        //PlayFabManager.OnGoldSpend += CheckEnergyUsed;  add function to check currencies usage
    }

    private void OnDisable()
    {
        MissionManager.OnMissionComplete -= CheckMission;
        BattleSystem.OnEnemyKilled -= CheckEnemyKilled;
        //PlayFabManager.OnGoldSpend -= CheckEnergyUsed;  add function to check currencies usage

    }
}