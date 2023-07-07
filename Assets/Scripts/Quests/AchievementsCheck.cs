using UnityEngine;

public class AchievementsCheck : MonoBehaviour
{
    //TODO -> Move to parent script
    public int goldAmount;
    public int crystalsAmount;

    //TODO -> Move to parent script
    public int dungeonNeeded;
    public int mainStoryNeeded;
    public int gearUpgradeNeeded;
    public int enemyKilledNeeded;
    public int energyUsedNeeded;

    //using events for every type of action in the game to manage the counts could be better

    //TODO -> Move to parent script
    int _dungeonCount;
    int _mainStoryCount;
    int _gearUpgradeCount;
    int _enemyKilledCount;
    int _energyUsedCount;


    private void CheckMission()
    {
        MissionSO mission = MissionManager.Instance.CurrentMission;

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


    private void CheckEnemyKilled(string name)
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
        PlayFabManager.Instance.AddCurrency(Currency.Gold, goldAmount);
        PlayFabManager.Instance.AddCurrency(Currency.Crystals, goldAmount);
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
