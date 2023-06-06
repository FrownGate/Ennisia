using System;

[Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int EquippedWeapon;
    public int[] EquippedGears;
    public int[] EquippedSupports;

    public PlayerData()
    {
        Level = 1;
        Exp = 0;
        EquippedWeapon = 0;
        EquippedGears = new int[6];
        EquippedSupports = new int[2];
    }
}