using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Game/Mission")]
public class MissionSO : ScriptableObject
{
    public int Id;
    public string Name;
    public MissionType Type;
    public MissionState State;
    public int EnergyCost;
    public bool Unlocked;
    public List<WavesSO> Waves;
    public int DialogueId;
    // Add additional mission data as needed
}
