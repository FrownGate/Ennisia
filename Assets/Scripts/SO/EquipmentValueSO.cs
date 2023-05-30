using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "EquipmentValue/New")]
public class EquipmentValueSO : ScriptableObject
{
    public int MinValue;
    public int MaxValue;
}