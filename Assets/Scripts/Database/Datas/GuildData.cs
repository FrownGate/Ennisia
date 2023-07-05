using PlayFab.DataModels;
using System;
using UnityEngine;

[Serializable]
public class GuildData
{
    public string Description;

    public GuildData() { }

    public GuildData(string description)
    {
        Description = description;
    }

    public GuildData(ObjectResult guild)
    {
        GuildData data = JsonUtility.FromJson<GuildData>(guild.EscapedDataObject);

        Description = data.Description;
    }
}