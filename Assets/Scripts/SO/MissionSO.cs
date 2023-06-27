using NaughtyAttributes;
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
    public List<string> Enemies;
    public int DialogueId;
    public int ChapterId;
    public int NumInChapter;
    [ShowNonSerializedField] public Dictionary<PlayFabManager.GameCurrency, int> RewardsList = new();
    public int Experience; 
    // Add additional mission data as needed
}