using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Ennisia/Effect")]
public class EffectSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int Duration;
    public bool Alteration;
    public bool Undispellable;
    [SerializedDictionary("Stat", "Modifier")] public SerializedDictionary<Attribute, float> StatsModifiers;
}