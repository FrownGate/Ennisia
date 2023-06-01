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
            ObjectName = GetName(),
            EscapedDataObject = JsonUtility.ToJson(this)
        };
    }

    public abstract void UpdateLocalData(string json);

    public string GetName()
    {
        return GetType().Name.Replace("Data", string.Empty);
    }
}
