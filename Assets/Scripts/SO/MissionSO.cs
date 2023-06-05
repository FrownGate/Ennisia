using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Ennisia/Mission")]
public class MissionSO : ScriptableObject
{
    public int Id;
    public string Name;
    public MissionManager.MissionType Type;
    public MissionManager.MissionState State;
    public int EnergyCost;
    public bool Unlocked;
    public Dictionary<int,string> Waves;
    public int WavesCount;
    public int DialogueId;
    public int ChapterId;
    public int NumInChapter;
    // Add additional mission data as needed
}