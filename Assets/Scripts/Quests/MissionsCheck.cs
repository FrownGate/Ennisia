using System;
using InfinityCode.UltimateEditorEnhancer.SceneTools;
using UnityEngine;
using UnityEngine.Rendering;

enum ChallengeType
{
    Daily, Weekly
}
public class MissionsCheck : MonoBehaviour
{
    bool[] dailies;
    bool[] weeklies;

    SerializedDictionary<MissionType, int> missionCount;
    SerializedDictionary<MissionType, int> missionNeeded;

    int _dailyGearUpgradeCount;
    int _dailyKillCount;
    int _dailyEnergyCount;

    public int goldAmount;
    public int crystalsAmount;

    public int _dailyKillNeeded;
    public int _dailyGearUpgradeNeeded;
    public int _dailyEnergyUsedNeeded;

    //TODO -> Move to parent script
    int _enemyKilledCount;
    int _energyUsedCount;

    private void GetMissionComplete(MissionSO missionSO)
    {
        missionCount[missionSO.Type]++;
        CheckChallengeMission(ChallengeType.Daily, missionSO, MissionType.Dungeon, 1);
        CheckChallengeMission(ChallengeType.Daily, missionSO, MissionType.Raid, 2);
        CheckChallengeMission(ChallengeType.Weekly, missionSO, MissionType.Dungeon, 1);
        CheckChallengeMission(ChallengeType.Weekly, missionSO, MissionType.Raid, 2);
    }

    private void CheckChallengeMission(ChallengeType challengeType, MissionSO missionSO, MissionType missionType, int challengeIndex)
    {
        if (missionSO.Type != missionType) return;
        switch (challengeType)
        {
            case ChallengeType.Daily:
                CheckChallengeDone(dailies[challengeIndex], missionCount[missionSO.Type], missionNeeded[missionSO.Type]);
                break;
            case ChallengeType.Weekly:
                CheckChallengeDone(weeklies[challengeIndex], missionCount[missionSO.Type], missionNeeded[missionSO.Type]);
                break;
        }
    }

    private void GetGeraUpgrade()
    {
        _dailyGearUpgradeCount++;
        CheckChallengeDone(dailies[3], _dailyGearUpgradeCount, _dailyGearUpgradeNeeded);
    }
    private void CheckChallengeDone(bool type, int count, int needed)
    {
        if (count < needed) return;
        type = true;
    }

    private void GetKill(string name)
    {
        _dailyKillCount++;
        CheckChallengeDone(dailies[4], _dailyKillCount, _dailyKillNeeded);
    }
    private void GetEnergyUsed()
    {
        _energyUsedCount++;
        CheckChallengeDone(weeklies[3], _energyUsedCount, _dailyEnergyUsedNeeded);
    }

    private void OnEnable()
    {
        MissionManager.OnMissionComplete += GetMissionComplete;
        Gear.LevelUp += GetGeraUpgrade;
        BattleSystem.OnEnemyKilled += GetKill;
    }
    private void OnDisable()
    {
        MissionManager.OnMissionComplete -= GetMissionComplete;
        Gear.LevelUp -= GetGeraUpgrade;
        BattleSystem.OnEnemyKilled -= GetKill;
    }




}

