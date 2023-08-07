using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCell : MonoBehaviour
{
    [SerializeField] private GameObject _itemHUD;
    [SerializeField] private GridLayoutGroup _layout;
    private List<Item> _itemSelected;
    private void Awake()
    {
        int j = 0;
        int k = 0;
        for (int i = 0; i < 10; i++)
        {
            GameObject gearCell = Instantiate(_itemHUD, _layout.transform);
            if (gearCell != null)
            {
                gearCell.transform.localPosition = new Vector3(200 + j, 200 + k);
            }
            j += 50;
            if (i % 5 == 0) k += 100;
        }
    }
    
    private void UpdateSelectedItems(Item itemToAdd, item itemToRemove)
    {
        _itemSelected.Remove(itemToRemove);
        _itemSelected.Add(itemToAdd);
    }

    private void OnEnable()
    {
        ItemSelection.OnItemSelected += UpdateSelectedItems();
    }
}
