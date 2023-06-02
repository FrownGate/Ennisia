using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public QuestManager questManager; // Référence au script QuestManager

    private void OnMouseDown()
    {
        if (!questManager.IsDailyQuestCompleted)
        {
            questManager.CompleteQuest("daily"); // Compléter la quête quotidienne
        }
        else if (!questManager.IsWeeklyQuestCompleted)
        {
            questManager.CompleteQuest("weekly"); // Compléter la quête hebdomadaire
        }
    }
}
