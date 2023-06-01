using PlayFab.DataModels;
using UnityEngine;
using System;

[Serializable]
public abstract class Data
{
    public SetObject Serialize()
    {
        return new SetObject
        {
            ObjectName = GetType().Name.Replace("Data", string.Empty),
            EscapedDataObject = JsonUtility.ToJson(this)
        };
    }

    public abstract void UpdateLocalData(string json);
}
