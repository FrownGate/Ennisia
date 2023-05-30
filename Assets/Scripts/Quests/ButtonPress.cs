using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public QuestManager questManager; // R�f�rence au script QuestManager

    private void OnMouseDown()
    {
        if (!questManager.IsDailyQuestCompleted)
        {
            questManager.CompleteQuest("daily"); // Compl�ter la qu�te quotidienne
        }
        else if (!questManager.IsWeeklyQuestCompleted)
        {
            questManager.CompleteQuest("weekly"); // Compl�ter la qu�te hebdomadaire
        }
    }
}
