using System;
using System.Runtime.InteropServices.WindowsRuntime;

enum ChallengeType
{
    Daily, Weekly
}
public class MissionsCheck
{
    bool[] dailies;
    bool[] weeklies;

    ChallengeType challengeType;

    public int goldAmount;
    public int crystalsAmount;
    //TODO -> Move to parent script
    public int enemyKilledNeeded;
    public int gearUpgradeNeeded;
    public int energyUsedNeeded;

    //TODO -> Move to parent script
    int _enemyKilledCount;
    int _gearUpgradeCount;
    int _energyUsedCount;

    private void CheckChallengeMission(ChallengeType challengeType, MissionSO mission, MissionManager.MissionType missionType, int count, int needed, int Index)
    {
        if (mission.Type == missionType)
        {
            count++;
            if(challengeType == ChallengeType.Daily)
            {
                CheckChallengeDone(dailies, count, needed, Index);
            }else if(challengeType == ChallengeType.Weekly) 
            {
                CheckChallengeDone(weeklies, count, needed, Index);
            }
        }
    }

    private void CheckChallengeGearUpgrade()
    {

    }
    private void CheckChallengeDone(bool[] type, int count, int needed, int challengeIndex)
    {
        if (count < needed) return;
        type[challengeIndex] = true;

    }

/*    private void CheckEnemyKilled(string name)
    {
        if (_enemyKilledCount >= enemyKilledNeeded)
        {
            daily?.Invoke(4);
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
            daily?.Invoke(5);
        }
        else
        {
            _energyUsedCount++;
        }
    }*/
}
