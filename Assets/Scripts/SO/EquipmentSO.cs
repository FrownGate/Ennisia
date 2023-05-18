using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment/New")]
public class EquipmentSO : ScriptableObject
{
    public int Id;
    public string equipmentName;
    public string type;
    public string rarity;
    public string attribute;
    public float value;
    public string description;
}
