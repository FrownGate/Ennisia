using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Ennisia/Equipment")]
public class GearSO : ScriptableObject
{
    public int Id = 1;
    public int Level = 1;
    public string Name;
    public Item.GearType Type;
    public Item.ItemRarity Rarity;
    public Item.AttributeStat Attribute;
    public float StatValue;
    public string Description;
    public Sprite Icon;
}