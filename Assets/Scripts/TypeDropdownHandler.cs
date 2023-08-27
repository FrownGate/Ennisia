using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeDropdownHandler : MonoBehaviour
{
    private void Awake()
    {
        TMP_Dropdown _dropdown = GetComponent<TMP_Dropdown>();
        
        _dropdown.options.Clear();
        foreach (GearType type in Enum.GetValues(typeof(GearType)))
        {
            if(type == GearType.Weapon)continue;
            _dropdown.options.Add(new TMP_Dropdown.OptionData(){text = type.ToString()});
        }
    }
}
