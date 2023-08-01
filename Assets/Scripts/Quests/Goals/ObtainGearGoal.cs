using System.Collections.Generic;
using System.Linq;

public class ObtainGearGoal : QuestSO.QuestGoal
{
    public List<GearType> Type = new();

    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<ObtainGearQuestEvent>(OnObtainGear);
    }

    private void OnObtainGear(ObtainGearQuestEvent eventInfo)
    {
        if (Completed) return;

        if (Type.Where(type => eventInfo.Type == type).Any()) CurrentAmount += 1;
        Evaluate();

    }
}