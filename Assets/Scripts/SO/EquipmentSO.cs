using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Ennisia/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public int Id = 1;
    public string Name;
    public Item.GearType Type;
    public Item.ItemRarity Rarity;
    public Item.AttributeStat Attribute;
    public string StatName;
    public float StatValue;
    public float StatUpgrade;
    public float RatioUpgrade;
    public int Level;
    public string Description;
    public Sprite Icon;

    //TODO -> Move Level and Upgrade system to Gear class

    public void Upgrade()
    {
        if (Level <= 50)
        {
            StatValue += (StatUpgrade * Level) + (StatValue * RatioUpgrade * Level);
        }
        else { }
    }

    public void Upgrade(int _level)
    {
        if (_level <= 50)
        {
            StatValue += (StatUpgrade * _level) + (StatValue * RatioUpgrade * _level);
        }
        else { }
    }
}