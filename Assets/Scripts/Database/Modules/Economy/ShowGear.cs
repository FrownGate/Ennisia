using System.Collections.Generic;

public class ShowGear : ItemHUD
{
    private List<Item> _items;
    private void Awake()
    {
        Gear gear = new Gear();
        _items = PlayFabManager.Instance.GetItems(gear);
        Init(_items[0]);
    }
}