using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Ennisia/Quest")]
public class QuestSO : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public int Energy;
    public SerializedDictionary<Currency, int> currencyList = new();
}