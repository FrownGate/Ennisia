using PlayFab.DataModels;
using UnityEngine;
using System;

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

    public SetObject Serialize()
    {
        return new SetObject
        {
            ObjectName = GetType().Name,
            EscapedDataObject = JsonUtility.ToJson(this)
        };
    }

    public void UpdateLocalData(string json)
    {
        Data data = JsonUtility.FromJson<Data>(json);
        Account = data.Account;
        Player = data.Player;
        Inventory = data.Inventory;

        Debug.Log($"User has {Inventory.Supports.Count} support(s).");
    }
}
