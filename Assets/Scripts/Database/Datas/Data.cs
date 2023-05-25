using PlayFab.DataModels;
using UnityEngine;
using System;

[Serializable]
public abstract class Data
{
    public string ClassName;
    public SetObject Serialize()
    {
        return new SetObject
        {
            ObjectName = ClassName,
            EscapedDataObject = JsonUtility.ToJson(this)
        };
    }

    public abstract void UpdateData(string json);
}
