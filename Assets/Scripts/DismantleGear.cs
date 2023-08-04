using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismantleGear : MonoBehaviour
{   
    [SerializeField] private GameObject _gear;
    private Item _item;

    public void Dismantle()
    {
        _item = _gear.GetComponent<ItemHUD>().Item;
        if (_item.Type != null)
        {
            PlayFabManager.Instance.Player.Unequip((GearType)_item.Type);
            Debug.Log("removing item");
            PlayFabManager.Instance.Inventory.Items.Remove(_item.Name);
        }else Debug.Log("item type null");
    }
}
