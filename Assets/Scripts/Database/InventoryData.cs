using System.Collections.Generic;
using UnityEngine;

public class InventoryData : Data
{
    public List<int> Supports;

    public InventoryData()
    {
        ClassName = "Inventory";
        Supports = new();
    }

    public override void UpdateData(string json)
    {
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);
        Supports = data.Supports;
    }
}
