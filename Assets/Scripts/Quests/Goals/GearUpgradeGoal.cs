public class GearUpgradeGoal : QuestSO.QuestGoal
{
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<GearUpgradeQuestEvent>(OnGearUpgrade);
    }

    private void OnGearUpgrade(GearUpgradeQuestEvent eventInfo)
    {
        if (Completed) return;
        CurrentAmount += 1;
        Evaluate();
    }
}