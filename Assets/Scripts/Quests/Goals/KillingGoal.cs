using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class KillingGoal : QuestSO.QuestGoal
{
    private string _killName;
    private SerializedDictionary<string, int> _killHistory = new();
    public List<EnemySO> ToKill = new();

    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<KillQuestEvent>(OnKilling);

        foreach (var enemy in ToKill.Where(enemy => !_killHistory.ContainsKey(enemy.Name)))
            _killHistory.Add(enemy.Name, 0);

#if UNITY_EDITOR
        //to reset the quest in the editor
        foreach (var enemy in ToKill) _killHistory[enemy.Name] = 0;
#endif
    }

    public override void Reset()
    {
        base.Reset();
        foreach (var enemy in ToKill) _killHistory[enemy.Name] = 0;
    }

    public void OnKilling(KillQuestEvent eventInfo)
    {
        if (Completed) return;
        foreach (var enemy in ToKill)
        {
            _killName = enemy.Name;
            if (eventInfo.KilledName != _killName) continue;
            if (Same) _killHistory[_killName] += 1;
            else CurrentAmount += 1;
        }

        if (Same)CurrentAmount = _killHistory.Values.Max();
        Evaluate();
    }
}