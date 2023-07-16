using UnityEngine;

public class SetRaidDifficulty : MonoBehaviour
{
    public MissionSO RaidDiff;

    private void OnMouseDown()
    {
        MissionManager.Instance.SetMission(RaidDiff);
    }
}