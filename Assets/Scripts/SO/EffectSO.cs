using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Ennisia/Effect")]
public class EffectSO : ScriptableObject
{
    public string Name;
    public string Description;
    public Dictionary<Item.AttributeStat, float> StatsModifiers;
    public AlterationState? Alteration;
    public int Duration;
}