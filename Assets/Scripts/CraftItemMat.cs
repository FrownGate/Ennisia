using TMPro;
using UnityEngine;
using System;

public class CraftItemMat : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _rarityDropdown;
    [SerializeField] private TMP_Dropdown _typeDropdown;
    [SerializeField] private int _amountNeeded;
    private Rarity _rarity;
    private GearType _type;
    
    public void CheckCraft()
    {
        foreach (var item in PlayFabManager.Instance.GetItems())
        {
            if (item is not Material)
                continue;
            if (item.Category == ItemCategory.Weapon) continue;
            int _rarityIndex = _rarityDropdown.value;
            _rarity = _rarityDropdown.options[_rarityIndex].text switch
            {
                "Common" => Rarity.Common,
                "Rare" => Rarity.Rare,
                "Epic" => Rarity.Epic,
                "Legendary" => Rarity.Legendary,
                _ => _rarity
            };
            int _typeIndex = _typeDropdown.value;
            _type = _typeDropdown.options[_typeIndex].text switch
            {
                "Helmet" => GearType.Helmet,
                "Chest" => GearType.Chest,
                "Boots" => GearType.Boots,
                "Necklace" => GearType.Necklace,
                "Earrings" => GearType.Earrings,
                "Ring" => GearType.Ring,
                _ => _type
            };
            if (item.Amount <= _amountNeeded)
            {
                Gear gear = new(_type, _rarity , null);
                PlayFabManager.Instance.AddInventoryItem(gear);
            }
        }
    }

    
}
