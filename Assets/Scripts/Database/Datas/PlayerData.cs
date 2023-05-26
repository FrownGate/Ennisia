using UnityEngine;

public class PlayerData : Data
{
    public int Level;
    public int Exp;
    public int EquippedWeapon;
    public int[] EquippedGears;
    public int[] EquippedSupports;

    public PlayerData()
    {
        ClassName = "Player";
        Level = 1;
        Exp = 0;
        EquippedWeapon = 0;
        EquippedGears = new int[6];
        EquippedSupports = new int[2];
    }

    public override void UpdateLocalData(string json)
    {
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        Level = data.Level;
        Exp = data.Exp;
        EquippedWeapon = data.EquippedWeapon;
        EquippedGears = data.EquippedGears;
        EquippedSupports = data.EquippedSupports;
    }
}