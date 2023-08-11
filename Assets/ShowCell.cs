using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCell : MonoBehaviour
{
    [SerializeField] private GameObject _itemHUD;
    [SerializeField] private GridLayoutGroup _layout;
    public List<Item> ItemSelected;
    public static ShowCell Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        for (int i = 0; i < 10; i++)
        {
            Instantiate(_itemHUD, _layout.transform);
        }
    }

    private void UpdateSelectedItems(Item itemToAdd, Item itemToRemove)
    {
        ItemSelected.Remove(itemToRemove);
        ItemSelected.Add(itemToAdd);
    }

    private void OnEnable()
    {
        ItemHUD.OnItemChange += UpdateSelectedItems;
    }

    private void OnDisable()
    {
        ItemHUD.OnItemChange -= UpdateSelectedItems;
    }
}