using System;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    public static event Action<Item> OnItemSelected;

    [SerializeField] private ItemHUD _hud;

    private void OnMouseUpAsButton()
    {
        Debug.Log($"Item {_hud.Item.Name} has been selected");
        OnItemSelected?.Invoke(_hud.Item);
    }
}