using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftItem : MonoBehaviour
{
    private List<Item> _selectedGearList;
    [SerializeField] private GameObject _button;
    
    
    private void Update()
    {
        _button.SetActive(CheckCrafting());
    }
    private void UpdateSelectedGear(Item item, Item item1)
    {
        var itemsCells = FindObjectsOfType<ShowGearCells>();
        List<Item> gearList = (from itemCell in itemsCells where itemCell.Item != null select itemCell.Item).ToList();

        _selectedGearList = gearList;
    }

    private bool CheckCrafting()
    {
        List<int> rarity = new List<int>();
        if (_selectedGearList.Count < 10) return false;
        foreach (var item in _selectedGearList)
        {
            switch (item.Rarity)
            {
                case Rarity.Common:
                    rarity[0] += 1;
                    break;
                case Rarity.Rare:
                    rarity[1] += 1;
                    break;
                case Rarity.Epic:
                    rarity[2] += 1;
                    break;
                case Rarity.Legendary:
                    rarity[3] += 1;
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return rarity.Any(qty => qty >= 10);
    }


    private void OnEnable()
    {
        ItemHUD.OnItemChange += UpdateSelectedGear;
    }

    private void OnDisable()
    {
        ItemHUD.OnItemChange -= UpdateSelectedGear;
    }
}