using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RarityDropdownHandler : MonoBehaviour
{
    private void Awake()
    {
        TMP_Dropdown _dropdown = GetComponent<TMP_Dropdown>();
        
        _dropdown.options.Clear();
        foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData(){text = rarity.ToString()});
        }
    }
}
