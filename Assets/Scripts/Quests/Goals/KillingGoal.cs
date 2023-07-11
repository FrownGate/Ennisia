using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingGoal : QuestSO.QuestGoal
{
    public string Kill;

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<KillGameEvent>(OnKilling);

    }

    public void OnKilling(KillGameEvent eventInfo)
    {
        if (eventInfo.KilledName != Kill) return;
        CurrentAmount++;
        Evaluate();
    }

}
