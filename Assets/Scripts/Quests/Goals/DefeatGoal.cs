
public class DefeatGoal : QuestSO.QuestGoal
{
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<DefeatQuestEvent>(OnDefeat);
    }

    private void OnDefeat(DefeatQuestEvent eventInfo)
    {
        if (Completed) return;
        if (eventInfo.Lost)
        {
            CurrentAmount += 1;

        }

        Evaluate();
    }
}