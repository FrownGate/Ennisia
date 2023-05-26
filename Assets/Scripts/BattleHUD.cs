using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] Slider hpSlider;
    
    public void SetHUD(Entity target)
    {
        /*nameText.text = target.entityName;
        lvlText.text = target.level.ToString();*/
        hpSlider.maxValue = target.MaxHp;
        hpSlider.value = target.CurrentHp;
    }

    public void SetHp(float hp)
    {
        hpSlider.value = hp;
    }

}
