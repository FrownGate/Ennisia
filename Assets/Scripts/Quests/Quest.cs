using UnityEngine;

public class Quest
{
    public QuestSO Data { get; protected set; }
    public string FileName { get; protected set; }

    public Quest()
    {
        FileName = GetType().Name;
        Data = Resources.Load<QuestSO>("SO/Quest/" + FileName);
    }

    public virtual void CheckCondition(string name)
    {
        //
    }

    public virtual void GiveRewards()
    {
        //PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, Data.crystlasAmount);
        //PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Gold, Data.goldAmount);
        //PlayFabManager.Instance.AddEnergy(Data.enrgyAmount);
    }
}