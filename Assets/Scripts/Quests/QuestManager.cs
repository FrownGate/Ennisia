using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int dailyButtonPressCount;
    private int weeklyButtonPressCount;

    private const int dailyButtonPressTarget = 10;
    private const int weeklyButtonPressTarget = 20;

    private bool dailyQuestCompleted;
    private bool weeklyQuestCompleted;

    private float lastDailyQuestCompletionTime;
    private float lastWeeklyQuestCompletionTime;

    private const float dailyQuestCooldownDuration = 24f * 60f * 60f; // Dur�e du cooldown pour la qu�te quotidienne en secondes
    private const float weeklyQuestCooldownDuration = 7f * 24f * 60f * 60f; // Dur�e du cooldown pour la qu�te hebdomadaire en secondes

    private void Start()
    {
        dailyButtonPressCount = 0;
        weeklyButtonPressCount = 0;

        dailyQuestCompleted = false;
        weeklyQuestCompleted = false;

        lastDailyQuestCompletionTime = PlayerPrefs.GetFloat("LastDailyQuestCompletionTime", 0f);
        lastWeeklyQuestCompletionTime = PlayerPrefs.GetFloat("LastWeeklyQuestCompletionTime", 0f);
    }

    public void OnButtonPress()
    {
        dailyButtonPressCount++;
        weeklyButtonPressCount++;

        if (dailyButtonPressCount >= dailyButtonPressTarget && !dailyQuestCompleted && CanCompleteDailyQuest())
        {
            CompleteDailyQuest();
        }

        if (weeklyButtonPressCount >= weeklyButtonPressTarget && !weeklyQuestCompleted && CanCompleteWeeklyQuest())
        {
            CompleteWeeklyQuest();
        }
    }

    private bool CanCompleteDailyQuest()
    {
        return Time.time >= lastDailyQuestCompletionTime + dailyQuestCooldownDuration;
    }

    private bool CanCompleteWeeklyQuest()
    {
        return Time.time >= lastWeeklyQuestCompletionTime + weeklyQuestCooldownDuration;
    }

    private void CompleteDailyQuest()
    {
        dailyQuestCompleted = true;
        lastDailyQuestCompletionTime = Time.time;
        PlayerPrefs.SetFloat("LastDailyQuestCompletionTime", lastDailyQuestCompletionTime);
        Debug.Log("Daily quest completed!");

        // Ajoutez ici le code pour r�compenser le joueur pour la qu�te quotidienne
        // R�initialisez �galement le compteur de pressions quotidiennes et mettez � jour l'�tat de la qu�te
        dailyButtonPressCount = 0;
    }

    private void CompleteWeeklyQuest()
    {
        weeklyQuestCompleted = true;
        lastWeeklyQuestCompletionTime = Time.time;
        PlayerPrefs.SetFloat("LastWeeklyQuestCompletionTime", lastWeeklyQuestCompletionTime);
        Debug.Log("Weekly quest completed!");

        // Ajoutez ici le code pour r�compenser le joueur pour la qu�te hebdomadaire
        // R�initialisez �galement le compteur de pressions hebdomadaires et mettez � jour l'�tat de la qu�te
        weeklyButtonPressCount = 0;
    }
}
