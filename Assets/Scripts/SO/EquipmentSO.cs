using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment/New")]
public class EquipmentSO : ScriptableObject
{
    public int Id = 1;
    public string Name;
    public string Type;
    public string Rarity;
    public string Attribute;
    public string StatName;
    public float StatValue;
    public float StatUpgrade;
    public float RatioUpgrade;
    public int Level;
    public string Description;
    public Sprite Icon;

    //TODO -> Move Level and Upgrade system to Gear class

    public int TypeIndex()
    {
        Dictionary<string, int> types = new()
        {
            { "Helmet", 0 }, { "Chest", 1 }, { "Boots", 2 }, { "Earrings", 3 }, { "Necklace", 4 }, { "Ring", 5 }
        };

        return types[Type];
    }

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