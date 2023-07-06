using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Ennisia/Enemy")]
public class EnemySO : SkillsToInitSO
{
    public int ID;
    public string Name;
    public string Description;
    public SerializedDictionary<Attribute, float> Stats = new();
}