using AYellowpaper.SerializedCollections;
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

    [SerializedDictionary("Number", "Enemies")] public SerializedDictionary<int, List<string>> Waves;
    public int WavesCount;
    public List<string> Enemies;
    public int DialogueId;
    public int ChapterId;
    public int NumInChapter;

    [SerializedDictionary("Currency", "Value")] public SerializedDictionary<Currency, int> CurrencyRewards = new();
    public List<RewardData> RewardData;
    public int Experience; 
    // Add additional mission data as needed
    public Sprite MissionBackground;

    public void UpdateData(MissionData data)
    {
        State = data.State;
        Unlocked = data.Unlocked;
    }
}