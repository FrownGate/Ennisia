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
    public string statName;
    public float statValue;
    public float statUpgrade;
    public float ratioUpgrade;
    public int level;
    public string Description;
    public Sprite Icon;

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
        if (level <= 50)
        {
            statValue += (statUpgrade * level) + (statValue * ratioUpgrade * level);
        }
        else { }
    }

    public void Upgrade(int _level)
    {
        if (_level <= 50)
        {
            statValue += (statUpgrade * _level) + (statValue * ratioUpgrade * _level);
        }
        else { }
    }
}