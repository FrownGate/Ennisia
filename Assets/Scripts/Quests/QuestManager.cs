using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestSO> Achievement;
    public List<QuestSO> Daily;
    public List<QuestSO> Weekly;

    public List<QuestSO> AllQuests;

    public static QuestManager Instance { get; private set; }


    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadQuest();
        foreach (var quest in AllQuests) quest.Initialize();

        
    }

    private void OnEnable()
    {
        BattleSystem.OnEnemyKilled += KillQuest;
        MissionManager.OnMissionComplete += MissionQuest;
        ExpManager.OnPlayerLevelUp += LevelUpQuest;
        ExpManager.OnAccountLevelUp += LevelUpQuest;
        Gear.LevelUp += OnGearUpgrade;
        Gear.MaxLevel += GearMaxLevelQuest;
        Gear.ObtainGear += ObtainGear;
        BattleSystem.OnPlayerLose += DefeatQuest;
        PlayFabManager.OnEnergyUsed += OnEnergyUsed;
        PlayFabManager.OnCurrencyUsed += OnCurrencyGain;
    }

    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= KillQuest;
        MissionManager.OnMissionComplete -= MissionQuest;
        ExpManager.OnPlayerLevelUp -= LevelUpQuest;
        ExpManager.OnAccountLevelUp -= LevelUpQuest;
        Gear.LevelUp -= OnGearUpgrade;
        Gear.MaxLevel -= GearMaxLevelQuest;
        BattleSystem.OnPlayerLose -= DefeatQuest;
        PlayFabManager.OnEnergyUsed -= OnEnergyUsed;
        PlayFabManager.OnCurrencyUsed -= OnCurrencyGain;

    }

    private void LoadQuest()
    {
        var questType = Enum.GetValues(typeof(QuestType));
        foreach (var type in questType)
        {
            var path = $"SO/Quests/{type}";
            switch (type)
            {
                case QuestType.Achievement:
                    Achievement.AddRange(Resources.LoadAll<QuestSO>(path));
                    break;
                case QuestType.Daily:
                    Daily.AddRange(Resources.LoadAll<QuestSO>(path));
                    break;
                case QuestType.Weekly:
                    Weekly.AddRange(Resources.LoadAll<QuestSO>(path));
                    break;
            }

            AllQuests.AddRange(Resources.LoadAll<QuestSO>(path));
        }
    }

    public bool IsQuestCompleted(QuestSO quest)
    {
        return quest.Completed;
    }

    public List<QuestSO> GetActiveQuests()
    {
        return AllQuests.Where(quest => !quest.Completed).ToList();
    }

    public void ResetQuestProgress()
    {
        foreach (var quest in AllQuests)
        {
            quest.Reset();

            // Reset the goals of the quest
            foreach (var goal in quest.Goals)
            {
                goal.Reset();
                goal.Initialize();
            }
        }
    }


    private void KillQuest(string name)
    {
        QuestEventManager.Instance.QueueEvent(new KillQuestEvent(name));
    }

    private void MissionQuest(MissionSO mission)
    {
        QuestEventManager.Instance.QueueEvent(new MissionQuestEvent(mission));
    }

    private void LevelUpQuest(int lvl,LevelUpQuestEvent.LvlType type)
    {
        QuestEventManager.Instance.QueueEvent(new LevelUpQuestEvent(lvl,type));
    }
    private void GearMaxLevelQuest(GearType? type)
    {
        QuestEventManager.Instance.QueueEvent(new GearLevelMaxQuestEvent(type));
    }

    private void DefeatQuest(bool lost)
    {
        QuestEventManager.Instance.QueueEvent(new DefeatQuestEvent(lost));
    }

    private void OnEnergyUsed(int amount)
    {
        QuestEventManager.Instance.QueueEvent(new EnergyQuestEvent(amount));
    }

    private void OnGearUpgrade()
    {
        QuestEventManager.Instance.QueueEvent(new GearUpgradeQuestEvent());
    }

    private void OnCurrencyGain(Currency currency,int amount)
    {
        QuestEventManager.Instance.QueueEvent(new CurrencyQuestEvent(currency,amount));
    }

    private void ObtainGear(GearType? type)
    {
        QuestEventManager.Instance.QueueEvent(new ObtainGearQuestEvent(type));
    }
}