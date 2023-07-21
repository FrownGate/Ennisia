using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class LevelUpGoal : QuestSO.QuestGoal
{
    public LevelUpQuestEvent.LvlType lvlType;
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<LevelUpQuestEvent>(OnLevelUP);
    }

    public void OnLevelUP(LevelUpQuestEvent eventInfo)
    {
        if (Completed) return;
        if (eventInfo.LevelType == lvlType) CurrentAmount = eventInfo.Level;
        Evaluate();
    }
}