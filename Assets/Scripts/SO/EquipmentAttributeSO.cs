using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "EquipmentAttribute/New")]
public class EquipmentAttributeSO : ScriptableObject
{
    public List<string> Attributes;
}