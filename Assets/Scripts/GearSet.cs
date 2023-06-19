using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GearSet : MonoBehaviour
{
    Item.GearSet?[] sets;

    private void CheckGearSet()
    {
        for(int i = 0; i < 7; i++)
        {
            //sets[i] = PlayFabManager.Instance.Player.EquippedGears[i].Set;
        }
    }
}

