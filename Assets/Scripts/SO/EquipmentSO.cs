using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment/New")]
public class EquipmentSO : ScriptableObject
{
    public int Id = 1;
    public string equipmentName;
    public string type;
    public string rarity;
    public string attribute;
    public float value;
    public string description;
    public Sprite Icon;

    public int TypeIndex()
    {
        Dictionary<string, int> types = new()
        {
            { "Helmet", 0 }, { "Chest", 1 }, { "Boots", 2 }, { "Earrings", 3 }, { "Necklace", 4 }, { "Ring", 5 }
        };

        return types[type];
    }
}