using System;
using System.Collections.Generic;
using static Item;

[Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int[] EquippedGearsId;
    public int[] EquippedSupports;
    [NonSerialized] public Dictionary<GearType, Gear> EquippedGears;

    public PlayerData()
    {
        Level = 1;
        Exp = 0;
        EquippedGearsId = new int[7];
        EquippedGears = new();
        EquippedSupports = new int[2];

        foreach (var item in Enum.GetNames(typeof(GearType)))
        {
            EquippedGears[Enum.Parse<GearType>(item)] = null;
        }
    }
}