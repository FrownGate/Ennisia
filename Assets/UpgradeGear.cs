using UnityEngine;

public class UpgradeGear : MonoBehaviour
{
    [SerializeField] private GameObject _gear;
    private Item _item;

    private void Start()
    {
        _item = _gear.GetComponent<ItemHUD>().Item;
        Debug.Log(_item.Name + "aieaieaiea");
    }

    public void UpgradeItem()
    {
        if (_item == null) {Debug.Log("item null"); return;}
        if (_item.Upgrade()) Debug.Log($"Item {_item.Name} upgraded");
    }
}