public class EnergyGoal : QuestSO.QuestGoal
{
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<EnergyQuestEvent>(OnEnergyUsed);
    }

    private void OnEnergyUsed(EnergyQuestEvent eventInfo)
    {
        if (Completed) return;
        CurrentAmount += eventInfo.Amount;
        Evaluate();
    }
}