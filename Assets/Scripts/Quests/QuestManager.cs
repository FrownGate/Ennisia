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

    private const float dailyQuestCooldownDuration = 24f * 60f * 60f; // Durée du cooldown pour la quête quotidienne en secondes
    private const float weeklyQuestCooldownDuration = 7f * 24f * 60f * 60f; // Durée du cooldown pour la quête hebdomadaire en secondes

    private float dailyQuestTimer;
    private float weeklyQuestTimer;

    private void Start()
    {
        dailyButtonPressCount = 0;
        weeklyButtonPressCount = 0;

        dailyQuestCompleted = false;
        weeklyQuestCompleted = false;

        lastDailyQuestCompletionTime = PlayerPrefs.GetFloat("LastDailyQuestCompletionTime", 0f);
        lastWeeklyQuestCompletionTime = PlayerPrefs.GetFloat("LastWeeklyQuestCompletionTime", 0f);

        dailyQuestTimer = 0f;
        weeklyQuestTimer = 0f;
    }

    private void Update()
    {
        if (!dailyQuestCompleted && CanCompleteDailyQuest())
        {
            Debug.Log("Daily quest is ready to be completed!");
        }

        if (!weeklyQuestCompleted && CanCompleteWeeklyQuest())
        {
            Debug.Log("Weekly quest is ready to be completed!");
        }
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

    private float GetTimeRemainingForDailyQuest()
    {
        return Mathf.Max(0f, lastDailyQuestCompletionTime + dailyQuestCooldownDuration - Time.time);
    }

    private float GetTimeRemainingForWeeklyQuest()
    {
        return Mathf.Max(0f, lastWeeklyQuestCompletionTime + weeklyQuestCooldownDuration - Time.time);
    }

    private string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private void CompleteDailyQuest()
    {
        dailyQuestCompleted = true;
        lastDailyQuestCompletionTime = Time.time;
        PlayerPrefs.SetFloat("LastDailyQuestCompletionTime", lastDailyQuestCompletionTime);
        Debug.Log("Daily quest completed!");

        // Ajoutez ici le code pour récompenser le joueur pour la quête quotidienne

        // Initialiser le compteur de pressions quotidiennes
        dailyButtonPressCount = 0;
    }

    private void CompleteWeeklyQuest()
    {
        weeklyQuestCompleted = true;
        lastWeeklyQuestCompletionTime = Time.time;
        PlayerPrefs.SetFloat("LastWeeklyQuestCompletionTime", lastWeeklyQuestCompletionTime);
        Debug.Log("Weekly quest completed!");

        // Ajoutez ici le code pour récompenser le joueur pour la quête hebdomadaire

        // Initialiser le compteur de pressions hebdomadaires
        weeklyButtonPressCount = 0;
    }
}
