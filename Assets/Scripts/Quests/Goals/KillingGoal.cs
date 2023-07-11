using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingGoal : QuestSO.QuestGoal
{
    private string _killName;
    private Dictionary<string, int> _killHistory = new();
    public List<EnemySO> ToKill;


    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<KillQuestEvent>(OnKilling);
        foreach (var enemy in ToKill)
        {
            _killHistory.Add(enemy.Name,0);
        }
    }

    public void OnKilling(KillQuestEvent eventInfo)
    {
        foreach (var enemy in ToKill)
        {
            _killName = enemy.Name;
            CurrentAmount = _killHistory[_killName];
            if (eventInfo.KilledName != _killName) continue;
            CurrentAmount++;
            _killHistory[_killName] = CurrentAmount;
            Evaluate();
        }
    }

}
