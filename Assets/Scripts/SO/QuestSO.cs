using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Ennisia/Quest")]
public class QuestSO : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public int Energy;
    [ShowNonSerializedField] public Dictionary<PlayFabManager.GameCurrency, int> currencyList = new();
}