
using UnityEngine;

public class LevelUpGoal : QuestSO.QuestGoal
{
    public LevelUpQuestEvent.LvlType lvlType;
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<LevelUpQuestEvent>(OnLevelUP);
    }

    private void OnLevelUP(LevelUpQuestEvent eventInfo)
    {
        if (Completed) return;
        Debug.Log("eventInfo.LevelType = " + eventInfo.LevelType);
        
        if (eventInfo.LevelType == lvlType) CurrentAmount = eventInfo.Level;
        Evaluate();
    }
}