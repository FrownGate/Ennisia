using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemHUD : MonoBehaviour
{
    public static event Action<Item, GearType?> OnSelectionOpened;

    [SerializeField] private Image _image;
    [SerializeField] protected bool _isSelectable;
    [SerializeField, Scene] private string _selectionScene;

    [Header("Item Datas")]
    [SerializeField] private bool _hasType;
    [SerializeField] private GearType _gearType;

    public Item Item { get; protected set; }

    public void Init(Item item)
    {
        Item = item;

        //TODO -> change sprite with item image
    }

    private void OnMouseUpAsButton()
    {
        if (!_isSelectable) return;
        ItemSelection.OnItemSelected += ChangeItem;
        ShowItems.OnPopupLoaded += SetSelection;
        ScenesManager.Instance.SetScene(_selectionScene);
    }

    private void SetSelection()
    {
        OnSelectionOpened?.Invoke(Item, _hasType ? _gearType : null);
        //TODO -> hide or show differently currently selected item
    }

    private void ChangeItem(Item item)
    {
        Debug.Log($"{item.Name} is now selected.");
        Init(item);
        ItemSelection.OnItemSelected -= ChangeItem;
        ShowItems.OnPopupLoaded -= SetSelection;
        ScenesManager.Instance.UnloadPopup(_selectionScene);
    }
}