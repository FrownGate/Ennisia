using UnityEngine;

public class UpgradeGear : MonoBehaviour
{
    [SerializeField] private GameObject _gear;
    private Item _item;

    public void UpgradeItem()
    {
        _item = _gear.GetComponent<ItemHUD>().Item;
        Debug.Log(_item.Name + "aieaieaiea");
        if (_item.Type == null) {Debug.Log("item type null"); return;}
        if (_item.Upgrade()) Debug.Log($"Item {_item.Name} upgraded");
    }
}