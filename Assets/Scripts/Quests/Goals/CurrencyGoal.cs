public class CurrencyGoal : QuestSO.QuestGoal
{
    public Currency CurrencyType;
    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<CurrencyQuestEvent>(OnCurrencyGain);
    }

    private void OnCurrencyGain(CurrencyQuestEvent eventInfo)
    {
        if (Completed) return;
        if(eventInfo.Currency == CurrencyType) CurrentAmount += eventInfo.Amount;
        
        Evaluate();
    }
}