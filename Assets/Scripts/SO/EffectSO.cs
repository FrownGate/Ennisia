using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Ennisia/Effect")]
public class EffectSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int Duration;
    public Effect.AlterationState Alteration;
    [SerializedDictionary("Stat", "Modifier")] public SerializedDictionary<Item.AttributeStat, float> StatsModifiers;
}