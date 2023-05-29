using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "GearValue/New")]
public class EquipmentValueSO : ScriptableObject
{
    public int MinValue;
    public int MaxValue;
}