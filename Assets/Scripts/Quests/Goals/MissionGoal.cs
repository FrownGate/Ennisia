using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class MissionGoal : QuestSO.QuestGoal
{
    private string _missionName;
    private SerializedDictionary<string, int> _missionHistory = new();
    public List<EnemySO> ToDo = new();

    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<MissionQuestEvent>(OnMissionDone);

        foreach (var enemy in ToDo.Where(enemy => !_missionHistory.ContainsKey(enemy.Name)))
        {
            _missionHistory.Add(enemy.Name, 0);
        }
    }


    public void OnMissionDone(MissionQuestEvent eventInfo)
    {
        foreach (var mission in ToDo)
        {
            _missionName = mission.Name;
            CurrentAmount = _missionHistory[_missionName];
            if (eventInfo.MissionName != _missionName) continue;
            CurrentAmount++;
            _missionHistory[_missionName] = CurrentAmount;
            Evaluate();
        }
    }

}
