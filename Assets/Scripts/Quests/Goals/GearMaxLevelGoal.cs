using System.Collections.Generic;
using System.Linq;

public class GearMaxLevelGoal : QuestSO.QuestGoal
{
    public List<GearType> Type = new();
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<GearLevelMaxQuestEvent>(OnMaxLevel);
    }

    public void OnMaxLevel(GearLevelMaxQuestEvent eventInfo)
    {
        if (Completed) return;
        if (Type.Where(type => eventInfo.Type == type).Any())
        {
            CurrentAmount += 1;
        }
        Evaluate();
    }
}
