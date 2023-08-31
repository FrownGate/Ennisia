using NaughtyAttributes;
using System;
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

    public void Init(Item item)
    {
        Item = item;

        _sprite = GetComponentInChildren<Image>();
        if (_sprite != null)
        {
            _sprite.sprite = Resources.Load<Sprite>($"Textures/empty");
            if (Item.Type != null) _sprite.sprite = Resources.Load<Sprite>($"Textures/Equipments/{Item.Name}");
        }

        if (Item is not Gear) return;
        Gear gear = Item as Gear;
        Debug.Log($"Attribute : {gear.Attribute}, value : {gear.Value}");
        _textValue.text = gear.Attribute + " " + gear.Value;
        _textLvl.text = gear.Level.ToString();
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