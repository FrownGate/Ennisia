using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImg;
    public Image hpEffectImg;
    
    public float buffTime = 0.8f; 

    public IEnumerator UpdateHpEffectCoroutine()
    {
        float effectLength = hpEffectImg.fillAmount - hpImg.fillAmount; 
        float elapsedTime = 0f; 

        while (elapsedTime < buffTime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime; 
            hpEffectImg.fillAmount = Mathf.Lerp(hpImg.fillAmount + effectLength, hpImg.fillAmount, elapsedTime / buffTime);
            yield return null;
        }

        hpEffectImg.fillAmount = hpImg.fillAmount; 
    }
    
}
