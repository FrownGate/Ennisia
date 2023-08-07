using System;
using System.Linq;
using PlayFab.ServerModels;
using UnityEngine;

public class UpgradeGear : MonoBehaviour
{
    public static event Action<Item> OnUpgraded;
    [SerializeField] private GameObject _gearGO;
    private Item _item;
    private Gear _gear;
    private int _matAmount;
    private int _matNeeded;

    public void UpgradeItem()
    {
        _item = _gearGO.GetComponent<ItemHUD>().Item;
        _gear = (Gear)_item;
        if (_item.Type == null)
        {
            Debug.Log("item type null");
            return;
        }

        Debug.Log(_item.Name + " attempting to upgrade");
        if (!CheckMaterials(_gear))
        {
            Debug.Log("not enought materials");
            return;
        }

        _item.Upgrade();
        OnUpgraded?.Invoke(_item);
        Debug.Log($"Item {_item.Name} upgraded");
    }

    private bool CheckMaterials(Gear gear)
    {
        Debug.Log("checking materials");
        if (gear.Category != null && gear.Rarity != null)
        {
            _matAmount = PlayFabManager.Instance.Inventory.GetMaterial((int)gear.Category, (int)gear.Rarity).Amount;
            Debug.Log(gear.Rarity + "materials available :" + _matAmount);
        }
        else
        {
            Debug.Log("category or rarity null, can't get materials available");
            return false;
        }
        _matNeeded = gear.Level * ((int)gear.Rarity + 1);
        Debug.Log(gear.Rarity + "mat needed :" + _matNeeded);
        if (_matAmount < _matNeeded)
        {
            Debug.Log("not enough materials");
            return false;
        }
        PlayFabManager.Instance.Inventory.RemoveItem(new Material(gear.Category.Value, gear.Rarity.Value, _matNeeded));
        return true;
    }
}