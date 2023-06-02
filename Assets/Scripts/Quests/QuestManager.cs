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

    public int dailyQuestClicksNeeded = 3; // Nombre de clics nécessaires pour compléter la quête quotidienne
    public int weeklyQuestClicksNeeded = 5; // Nombre de clics nécessaires pour compléter la quête hebdomadaire

    private int buttonClickCount = 0; // Compteur de clics

    private bool isDailyQuestCompleted = false; // Indique si la quête quotidienne est complétée
    private bool isWeeklyQuestCompleted = false; // Indique si la quête hebdomadaire est complétée

    private bool isDailyQuestOnCooldown = false; // Indique si la quête quotidienne est en cooldown
    private bool isWeeklyQuestOnCooldown = false; // Indique si la quête hebdomadaire est en cooldown

    public float dailyQuestCooldownDuration = 10f; // Durée du cooldown de la quête quotidienne en secondes
    public float weeklyQuestCooldownDuration = 20f; // Durée du cooldown de la quête hebdomadaire en secondes

    private float dailyQuestCooldownTimer = 0f; // Timer du cooldown de la quête quotidienne
    private float weeklyQuestCooldownTimer = 0f; // Timer du cooldown de la quête hebdomadaire

    private void Update()
    {
        // Gestion du cooldown de la quête quotidienne
        if (isDailyQuestOnCooldown)
        {
            dailyQuestCooldownTimer -= Time.deltaTime;
            if (dailyQuestCooldownTimer <= 0f)
            {
                isDailyQuestOnCooldown = false;
                Debug.Log("Le cooldown de la quête quotidienne est terminé. Vous pouvez commencer la quête à nouveau.");
            }
        }

        // Gestion du cooldown de la quête hebdomadaire
        if (isWeeklyQuestOnCooldown)
        {
            weeklyQuestCooldownTimer -= Time.deltaTime;
            if (weeklyQuestCooldownTimer <= 0f)
            {
                isWeeklyQuestOnCooldown = false;
                Debug.Log("Le cooldown de la quête hebdomadaire est terminé. Vous pouvez commencer la quête à nouveau.");
            }
        }
    }

    public void OnButtonPress()
    {
        // Vérification de la complétion de la quête quotidienne
        if (isDailyQuestCompleted && !isDailyQuestOnCooldown)
        {
            buttonClickCount++;

            if (buttonClickCount >= dailyQuestClicksNeeded)
            {
                CompleteQuest("daily");
                StartQuestCooldown("daily", dailyQuestCooldownDuration);
            }
        }

        // Vérification de la complétion de la quête hebdomadaire
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
        // Réinitialisation de la quête spécifiée
        switch (questType)
        {
            case "daily":
                isDailyQuestCompleted = true; // Marquer la quête quotidienne comme complétée
                Debug.Log("Quête quotidienne complétée !");
                break;
            case "weekly":
                isWeeklyQuestCompleted = true; // Marquer la quête hebdomadaire comme complétée
                Debug.Log("Quête hebdomadaire complétée !");
                break;
            default:
                Debug.LogError("Type de quête invalide !");
                break;
        }

        // Réinitialisation du compteur de clics
        buttonClickCount = 0;
    }

    private void StartQuestCooldown(string questType, float cooldownDuration)
    {
        // Lancement du cooldown de la quête spécifiée
        switch (questType)
        {
            case "daily":
                isDailyQuestOnCooldown = true;
                dailyQuestCooldownTimer = cooldownDuration;
                Debug.Log("Le cooldown de la quête quotidienne est en cours. Attendez " + cooldownDuration + " secondes.");
                break;
            case "weekly":
                isWeeklyQuestOnCooldown = true;
                weeklyQuestCooldownTimer = cooldownDuration;
                Debug.Log("Le cooldown de la quête hebdomadaire est en cours. Attendez " + cooldownDuration + " secondes.");
                break;
            default:
                Debug.LogError("Type de quête invalide !");
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
