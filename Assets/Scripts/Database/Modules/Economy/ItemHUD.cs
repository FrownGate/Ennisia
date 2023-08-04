using NaughtyAttributes;
using System;
using System.Globalization;
using InfinityCode.UltimateEditorEnhancer.SceneTools;
using InfinityCode.UltimateEditorEnhancer.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHUD : MonoBehaviour
{
    public static event Action<Item, GearType?> OnSelectionOpened;

    private Image _sprite;
    private TextMeshProUGUI _textValue;
    [SerializeField] protected bool _isSelectable;
    [SerializeField, Scene] private string _selectionScene;

    [Header("Item Datas")] [SerializeField]
    private bool _hasType;

    [SerializeField] private GearType _gearType;
    public Item Item { get; protected set; }
    public Gear Gear { get; protected set; }

    public void Init(Item item)
    {
        Item = item;
        if (item is Gear gear) Gear = gear;
        
        _sprite = GetComponentInChildren<Image>();
        _sprite.sprite = Resources.Load<Sprite>($"Textures/Equipments/Default");
        if (Item.Type != null) _sprite.sprite = Resources.Load<Sprite>($"Textures/Equipments/{Item.Name}");
        _textValue = GetComponentInChildren<TextMeshProUGUI>();
        if (Gear == null) return;
        Debug.Log(Gear.Attribute + " " + Gear.Value);
        _textValue.text = Gear.Value.ToString(CultureInfo.CurrentCulture);
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