using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Game/Mission")]
public class MissionSO : ScriptableObject
{
    public int ID;
    public string Name;
    public MissionType Type;
    public MissionState State;
    public int EnergyCost;
    public bool Unlocked;
    public Dictionary<int,string> Waves;
    public int WavesCount;
    public int DialogueId;
    public int ChapID;
    // Add additional mission data as needed
}
