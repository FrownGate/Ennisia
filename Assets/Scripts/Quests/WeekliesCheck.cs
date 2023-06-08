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

    public int goldAmount;
    public int crystalsAmount;

    public int mainStoryNeeded;
    public int raidNeeded;
    public int expeditionNeeded;
    public int enemyKilledNeeded;

    int _mainStoryCount;
    int _raidCount;
    int _expeditionCount;
    int _enemyKilledCount;



    private void CheckMission(MissionSO mission)
    {
        if (mission.Type == MissionManager.MissionType.MainStory)
        {
            _mainStoryCount++;
        }
        if (mission.Type == MissionManager.MissionType.Raid)
        {
            _raidCount++;
        }
        if (mission.Type == MissionManager.MissionType.Expedition)
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



    private void CheckEnemyKilled()
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
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Gold, goldAmount);
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, goldAmount);
    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += CheckMission;
        BattleSystem.enemyKilled += CheckEnemyKilled;
        //PlayFabManager.OnGoldSpend += CheckEnergyUsed;  add function to check currencies usage
    }

    private void OnDisable()
    {
        MissionManager.OnMissionComplete -= CheckMission;
        BattleSystem.enemyKilled -= CheckEnemyKilled;
        //PlayFabManager.OnGoldSpend -= CheckEnergyUsed;  add function to check currencies usage

    }
}