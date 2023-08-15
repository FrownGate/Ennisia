using UnityEngine;
using System;
using System.Text;

[Serializable]
public class Data
{
    public AccountData Account;
    public PlayerData Player;
    public InventoryData Inventory;
    [NonSerialized] public bool HasMissingDatas;

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
        //Debug.LogWarning($"{Account.MissionsData.Count}");

        HasMissingDatas = Account.GetMissionsData();
    }

    public void UpdateEquippedGears()
    {
        int index = 0;
        int[] gears = new int[Player.EquippedGearsId.Length];
        bool changes = false;

        Debug.LogWarning("PLAYER EQUIPMENTS");

        foreach (var id in Player.EquippedGearsId)
        {
            if (id != 0)
            {
                Debug.Log(id);
                Gear gear = Inventory.GetGearById(id);

                if (gear == null)
                {
                    changes = true;
                    gears[index] = 0;
                    Debug.Log($"No equipped gear on {(GearType)index} slot.");
                    continue;
                }

                Player.Equip(gear, false);
                Debug.Log($"Equipped {gear.Type} = {gear.Name}");
            }
            else
            {
                Debug.Log($"No equipped gear on {(GearType)index} slot.");
            }

            index++;
        }

        if (changes)
        {
            Player.EquippedGearsId = gears;
            PlayFabManager.Instance.UpdateData();
        }

        Debug.LogWarning("PLAYER STATS");
        foreach (var stat in Player.Stats) Debug.Log($"{stat.Key} : {stat.Value.Value}");
    }
}