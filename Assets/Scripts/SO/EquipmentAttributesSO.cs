using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentAttributes", menuName = "Ennisia/Equipment Attributes")]
public class EquipmentAttributesSO : ScriptableObject
{
    public List<Item.AttributeStat> Attributes;
}