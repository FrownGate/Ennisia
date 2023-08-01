using UnityEngine;

public class UpgradeGear : MonoBehaviour
{
    [SerializeField] private ShowGear _item;

    private void Start()
    {
        //_item = FindObjectOfType<ShowGear>().Item;
    }

    public void UpgradeItem()
    {
        if (_item == null) return;
        if (_item.Item.Upgrade()) Debug.Log($"Item {_item.Item.Name} upgraded");

    }
}