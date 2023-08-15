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
    public static event Action<Item, Item> OnItemChange;

    private Image _sprite;
    [SerializeField] private TextMeshProUGUI _textValue;
    [SerializeField] private TextMeshProUGUI _textLvl;
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
        if (_sprite != null)
        {
            _sprite.sprite = Resources.Load<Sprite>($"Textures/Equipments/Default");
            if (Item.Type != null) _sprite.sprite = Resources.Load<Sprite>($"Textures/Equipments/{Item.Name}");
        }

        if (Gear == null) return;
        Debug.Log(Gear.Attribute + " " + Gear.Value);
        _textValue.text = Gear.Attribute + " " + Gear.Value;
        _textLvl.text = Gear.Level.ToString();
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
        OnItemChange?.Invoke(item, Item);
        Init(item);
        ItemSelection.OnItemSelected -= ChangeItem;
        ShowItems.OnPopupLoaded -= SetSelection;
        ScenesManager.Instance.UnloadPopup(_selectionScene);
    }

    private void UpdateHUD(Item item)
    {
        Init(item);
        Debug.Log("HUD updated !");
    }

    private void OnEnable()
    {
        UpgradeGear.OnUpgraded += UpdateHUD;
    }

    private void OnDisable()
    {
        UpgradeGear.OnUpgraded -= UpdateHUD;
    }
}