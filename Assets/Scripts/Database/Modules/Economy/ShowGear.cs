using System.Collections.Generic;
using System.Linq;

public class ShowGear : ItemHUD
{
    private Item _item;
    private void Awake()
    {
        Gear gear = new Gear();
        _item = PlayFabManager.Instance.GetItems(gear)[0];
        Init(_item);
    }
}