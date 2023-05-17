using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment/New")]
public class EquipmentSO : ScriptableObject
{
    public string Name;
    public string Type;
    public string Rarity;
    public string Attribute;
    public float Value;
    public string Description;
}
