using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> dailyQuests;
    public List<Quest> weeklyQuests;

    private void Start()
    {
        GenerateDailyQuests();
        GenerateWeeklyQuests();
    }

    private void GenerateDailyQuests()
    {
        // Generate daily quests here
    }

    private void GenerateWeeklyQuests()
    {
        // Generate weekly quests here
    }

    public void CheckQuestCompletion(Quest quest)
    {
        // Check if the quest is completed and update its status
    }

    public void GrantReward(Quest quest)
    {
        // Grant the reward for the completed quest
    }
}
