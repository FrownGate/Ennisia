using System.Linq;
using PlayFab.ServerModels;
using UnityEngine;

public class UpgradeGear : MonoBehaviour
{
    [SerializeField] private GameObject _gearGO;
    private Item _item;
    private Gear _gear;
    private int _matAmount;
    private int _matNeeded;

    public void UpgradeItem()
    {
        _item = _gearGO.GetComponent<ItemHUD>().Item;
        Debug.Log(_item.Name + "aieaieaiea");
        if (_item.Type == null)
        {
            Debug.Log("item type null");
            return;
        }

        if (!CheckMaterials())
        {
            Debug.Log("not enought materials");
            return;
        }

        _item.Upgrade();
        Debug.Log($"Item {_item.Name} upgraded");
    }

    private bool CheckMaterials()
    {
        _matAmount = PlayFabManager.Instance.Inventory.GetMaterial(1, 3).Amount;
        Debug.Log(_matAmount);
        if (_item is not Gear gear) return _matAmount >= _matNeeded;
        _gear = gear;
        if (gear.Rarity != null) _matNeeded = gear.Level * ((int)gear.Rarity + 1);
        Debug.Log(_matNeeded);
        if (_matAmount < _matNeeded) return false;
        PlayFabManager.Instance.Inventory.RemoveItem(new Material(ItemCategory.Armor, Rarity.Legendary,
            _matNeeded));
        return true;
    }
}