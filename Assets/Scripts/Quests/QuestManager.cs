using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //TODO -> ajouter instance QuestManager

    //No need to create timers -> We'll use PlayFab to reset timers at specific date and time
    //Daily timer around 16:00
    //Weekly timer around 16:00 on Monday

    //Only check in database if quest is completed or not
    //Set quest state -> same system as Missions
    //Give rewards to player depending on CSV datas ?

    public int dailyQuestClicksNeeded = 3; // Nombre de clics n�cessaires pour compl�ter la qu�te quotidienne
    public int weeklyQuestClicksNeeded = 5; // Nombre de clics n�cessaires pour compl�ter la qu�te hebdomadaire

    private int buttonClickCount = 0; // Compteur de clics

    private bool isDailyQuestCompleted = false; // Indique si la qu�te quotidienne est compl�t�e
    private bool isWeeklyQuestCompleted = false; // Indique si la qu�te hebdomadaire est compl�t�e

    private bool isDailyQuestOnCooldown = false; // Indique si la qu�te quotidienne est en cooldown
    private bool isWeeklyQuestOnCooldown = false; // Indique si la qu�te hebdomadaire est en cooldown

    public float dailyQuestCooldownDuration = 10f; // Dur�e du cooldown de la qu�te quotidienne en secondes
    public float weeklyQuestCooldownDuration = 20f; // Dur�e du cooldown de la qu�te hebdomadaire en secondes

    private float dailyQuestCooldownTimer = 0f; // Timer du cooldown de la qu�te quotidienne
    private float weeklyQuestCooldownTimer = 0f; // Timer du cooldown de la qu�te hebdomadaire

    private void Update()
    {
        // Gestion du cooldown de la qu�te quotidienne
        if (isDailyQuestOnCooldown)
        {
            dailyQuestCooldownTimer -= Time.deltaTime;
            if (dailyQuestCooldownTimer <= 0f)
            {
                isDailyQuestOnCooldown = false;
                Debug.Log("Le cooldown de la qu�te quotidienne est termin�. Vous pouvez commencer la qu�te � nouveau.");
            }
        }

        // Gestion du cooldown de la qu�te hebdomadaire
        if (isWeeklyQuestOnCooldown)
        {
            weeklyQuestCooldownTimer -= Time.deltaTime;
            if (weeklyQuestCooldownTimer <= 0f)
            {
                isWeeklyQuestOnCooldown = false;
                Debug.Log("Le cooldown de la qu�te hebdomadaire est termin�. Vous pouvez commencer la qu�te � nouveau.");
            }
        }
    }

    public void OnButtonPress()
    {
        // V�rification de la compl�tion de la qu�te quotidienne
        if (isDailyQuestCompleted && !isDailyQuestOnCooldown)
        {
            buttonClickCount++;

            if (buttonClickCount >= dailyQuestClicksNeeded)
            {
                CompleteQuest("daily");
                StartQuestCooldown("daily", dailyQuestCooldownDuration);
            }
        }

        // V�rification de la compl�tion de la qu�te hebdomadaire
        if (isWeeklyQuestCompleted && !isWeeklyQuestOnCooldown)
        {
            buttonClickCount++;

            if (buttonClickCount >= weeklyQuestClicksNeeded)
            {
                CompleteQuest("weekly");
                StartQuestCooldown("weekly", weeklyQuestCooldownDuration);
            }
        }
    }

    public void CompleteQuest(string questType)
    {
        // R�initialisation de la qu�te sp�cifi�e
        switch (questType)
        {
            case "daily":
                isDailyQuestCompleted = true; // Marquer la qu�te quotidienne comme compl�t�e
                Debug.Log("Qu�te quotidienne compl�t�e !");
                break;
            case "weekly":
                isWeeklyQuestCompleted = true; // Marquer la qu�te hebdomadaire comme compl�t�e
                Debug.Log("Qu�te hebdomadaire compl�t�e !");
                break;
            default:
                Debug.LogError("Type de qu�te invalide !");
                break;
        }

        // R�initialisation du compteur de clics
        buttonClickCount = 0;
    }

    private void StartQuestCooldown(string questType, float cooldownDuration)
    {
        // Lancement du cooldown de la qu�te sp�cifi�e
        switch (questType)
        {
            case "daily":
                isDailyQuestOnCooldown = true;
                dailyQuestCooldownTimer = cooldownDuration;
                Debug.Log("Le cooldown de la qu�te quotidienne est en cours. Attendez " + cooldownDuration + " secondes.");
                break;
            case "weekly":
                isWeeklyQuestOnCooldown = true;
                weeklyQuestCooldownTimer = cooldownDuration;
                Debug.Log("Le cooldown de la qu�te hebdomadaire est en cours. Attendez " + cooldownDuration + " secondes.");
                break;
            default:
                Debug.LogError("Type de qu�te invalide !");
                break;
        }
    }

    public bool IsDailyQuestCompleted
    {
        get { return isDailyQuestCompleted; }
    }

    public bool IsWeeklyQuestCompleted
    {
        get { return isWeeklyQuestCompleted; }
    }
}
