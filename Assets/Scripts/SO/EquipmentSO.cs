using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment/New")]
public class EquipmentSO : ScriptableObject
{
    public string Name;
    public string Type;
    public float CommonMin;
    public float CommonMax;
    public float RareMin;
    public float RareMax;
    public float SuperRareMin;
    public float SuperRareMax;
    public float SuperSuperRareMin;
    public float SuperSuperRareMax;
}
