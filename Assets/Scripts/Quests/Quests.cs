using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{

    public QuestSO Data { get; protected set; }
    public string FileName { get; protected set; }

    public Quests()
    {
        FileName = GetType().Name;
        Data = Resources.Load<QuestSO>("SO/Quest/" + FileName);
    }

    public virtual void CheckCondition()
    {

    }

    public virtual void GiveRewards()
    {
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Crystals, Data.crystlasAmount);
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Gold, Data.goldAmount);
        PlayFabManager.Instance.AddEnergy(Data.enrgyAmount);
    }

}
