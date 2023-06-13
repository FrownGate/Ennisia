using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int[] EquippedGearsId;
    public Dictionary<Item.GearType, Gear> EquippedGears;
    public int[] EquippedSupports;

    public PlayerData()
    {
        Level = 1;
        Exp = 0;
        EquippedGearsId = new int[7];
        EquippedGears = new();
        EquippedSupports = new int[2];

        foreach (var item in Enum.GetNames(typeof(Item.GearType)))
        {
            EquippedGears[Enum.Parse<Item.GearType>(item)] = null;
        }
    }
}