using System;

[Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int[] EquippedGears;
    public int[] EquippedSupports;

    public PlayerData()
    {
        Level = 1;
        Exp = 0;
        EquippedGears = new int[7];
        EquippedSupports = new int[2];
    }
}