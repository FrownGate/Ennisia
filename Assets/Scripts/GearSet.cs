using System;
using UnityEngine;

public class GearSet : MonoBehaviour
{
    Gear[] _sets;
    [HideInInspector] public Skill FourPieces;
    [HideInInspector] public Skill SixPieces;

    private void CheckGearSet()
    {
        int wui = 0;
        int index = 0;
        foreach (var gear in PlayFabManager.Instance.Player.EquippedGears)
        {
            _sets[index] = gear.Value;
            index++;
        }

        for (int i = 0; i < _sets.Length; i++)
        {
            for (int j = 0; j < _sets.Length; j++)
            {
                if (_sets[i] == _sets[j])
                {
                    wui++;
                    if(wui == 4)
                    {
                        Type type = Type.GetType(CSVUtils.GetFileName(_sets[i].ToString()));
                        FourPieces = (Skill)Activator.CreateInstance(type);
                        if (wui == 6)
                        {
                            Type type2 = Type.GetType(CSVUtils.GetFileName(_sets[i].ToString() + "6"));
                            SixPieces = (Skill)Activator.CreateInstance(type);
                        }
                    }     
                }
            }
        }
    }
}