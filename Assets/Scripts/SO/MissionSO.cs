using AYellowpaper.SerializedCollections;
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
    [SerializedDictionary("Number", "Enemies")]
    public SerializedDictionary<int, List<string>> Waves;
    public int WavesCount;
    public List<string> Enemies;
    public int DialogueId;
    public int ChapterId;
    public int NumInChapter;
    [SerializedDictionary("Currency", "Value")]
    public SerializedDictionary<PlayFabManager.GameCurrency, int> CurrencyRewards = new();
    public List<Item.ItemRarity> GearReward;
    public int Experience; 
    // Add additional mission data as needed
}