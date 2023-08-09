using System.Collections.Generic;
using System.Linq;

public class ShowGearCells : ItemHUD
{
    private Item _item;
    private void Awake()
    {
        Init(new Gear());
    }
}
