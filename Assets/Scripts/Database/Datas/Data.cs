using UnityEngine;
using System;
using System.Text;

[Serializable]
public class Data
{
    public AccountData Account;
    public PlayerData Player;
    public InventoryData Inventory;

    public Data(string username)
    {
        Account = new(username);
        Player = new();
        Inventory = new();
    }

    public byte[] Serialize()
    {
        return Encoding.UTF8.GetBytes(JsonUtility.ToJson(this));
    }

    public void UpdateLocalData(string json)
    {
        Data data = JsonUtility.FromJson<Data>(json);
        Account = data.Account;
        Player = data.Player;
        Inventory = data.Inventory;

        Player.UpdateEquippedSupports();
        Player.UpdatePlayerStats();

        Debug.Log($"User has {Inventory.Supports.Count} support(s).");
    }

    public void UpdateEquippedGears()
    {
        foreach (var id in Player.EquippedGearsId)
        {
            if (id != 0)
            {
                Gear gear = Inventory.GetGearById(id);
                Player.Equip(gear, false);
                Debug.Log($"Equipped {gear.Type} = {gear.Name}");
            }
        }

        foreach (var stat in Player.Stats) Debug.Log($"{stat.Key} : {stat.Value.Value}");
    }
}