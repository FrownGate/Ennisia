using System.Collections.Generic;
using UnityEngine;

public class InventoryData : Data
{
    public List<SupportData> Supports;

    public InventoryData()
    {
        ClassName = "Inventory";
        Supports = new();
    }

    public override void UpdateLocalData(string json)
    {
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);
        Supports = data.Supports;
    }
}
