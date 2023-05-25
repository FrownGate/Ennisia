using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int dailyButtonPressCount;
    private int weeklyButtonPressCount;

    private const int dailyButtonPressTarget = 10;
    private const int weeklyButtonPressTarget = 20;

    private bool dailyQuestCompleted;
    private bool weeklyQuestCompleted;

    private void Start()
    {
        dailyButtonPressCount = 0;
        weeklyButtonPressCount = 0;

        dailyQuestCompleted = false;
        weeklyQuestCompleted = false;
    }

    public void OnButtonPress()
    {
        dailyButtonPressCount++;
        weeklyButtonPressCount++;

        if (dailyButtonPressCount >= dailyButtonPressTarget && !dailyQuestCompleted)
        {
            CompleteDailyQuest();
        }

        if (weeklyButtonPressCount >= weeklyButtonPressTarget && !weeklyQuestCompleted)
        {
            CompleteWeeklyQuest();
        }
    }

    private void CompleteDailyQuest()
    {
        dailyQuestCompleted = true;
        Debug.Log("Daily quest completed!");
        // Ajoutez ici le code pour récompenser le joueur pour la quête quotidienne
    }

    private void CompleteWeeklyQuest()
    {
        weeklyQuestCompleted = true;
        Debug.Log("Weekly quest completed!");
        // Ajoutez ici le code pour récompenser le joueur pour la quête hebdomadaire
    }
}
