using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager s_Instance = null;

    public List<QuestSO> CurrentQuests;


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

        foreach (var quest in CurrentQuests)
        {
            quest.Initialize();
        }

        BattleSystem.OnEnemyKilled += KillQuest;
        MissionManager.OnMissionComplete += MissionQuest;
    }

    public void KillQuest(string name)
    {
        QuestEventManager.Instance.QueueEvent(new KillQuestEvent(name));
    }

    public void MissionQuest(MissionSO mission)
    {

    }
}
