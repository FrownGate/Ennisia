using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class MissionGoal : QuestSO.QuestGoal
{
    private MissionSO _mission;
    private SerializedDictionary<string, int> _missionHistory = new();
    public List<MissionSO> ToDo = new();

    public override void Initialize()
    {
        base.Initialize();
        QuestEventManager.Instance.AddListener<MissionQuestEvent>(OnMissionDone);

        foreach (var mission in ToDo.Where(mission =>    !_missionHistory.ContainsKey(mission.Name)))
            _missionHistory.Add(mission.Name, 0);

#if UNITY_EDITOR
        //to reset the quest in the editor
        foreach (var mission in ToDo) _missionHistory[mission.Name] = 0;
#endif
    }

    public override void Reset()
    {
        base.Reset();
        foreach (var mission in ToDo) _missionHistory[mission.Name] = 0;
    }

    public void OnMissionDone(MissionQuestEvent eventInfo)
    {
        if (Completed) return;
        foreach (var mission in ToDo)
        {
            _mission = mission;
            if (eventInfo.Mission != _mission) continue;
            if (Same) _missionHistory[_mission.Name] += 1;
            else CurrentAmount += 1;
        }

        if (Same)CurrentAmount = _missionHistory.Values.Max();
        Evaluate();
    }
}