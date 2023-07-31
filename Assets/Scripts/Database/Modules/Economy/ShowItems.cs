using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItems : MonoBehaviour
{
    public static event Action OnPopupLoaded;

    [SerializeField] private GameObject _itemHUD;
    [SerializeField] private GridLayoutGroup _layout;

    private List<Item> _items;

    private void Awake()
    {
        ItemHUD.OnSelectionOpened += Show;
        OnPopupLoaded?.Invoke();
    }

    private void OnDestroy()
    {
        ItemHUD.OnSelectionOpened -= Show;
    }

    public void Show(Item category, GearType? gearType)
    {
        _items = PlayFabManager.Instance.GetItems(category);

        foreach (var item in _items)
        {
            if (gearType != null && item.Type != gearType) continue;
            Debug.Log(item.Name);
            GameObject itemObject = Instantiate(_itemHUD, _layout.transform);
            itemObject.GetComponent<ItemHUD>().Init(item);
        }
    }
}