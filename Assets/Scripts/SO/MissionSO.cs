using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Ennisia/Mission")]
public class MissionSO : ScriptableObject
{
    public int Id;
    public string Name;
    public MissionType Type;
    public MissionState State;
    public int EnergyCost;
    public bool Unlocked;
    public Dictionary<int,string> Waves;
    public int WavesCount;
    public int DialogueId;
    public int ChapterId;
    // Add additional mission data as needed
}