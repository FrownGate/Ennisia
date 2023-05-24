using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpellClick : MonoBehaviour
{
    public int spellNumber = 0;
    public int damage;
    public int cooldown;
    public int maxCooldown;
    public int aoeCheck;

    public static event Action<int> OnClick;

    public void OnMouseDown()
    {
        OnClick?.Invoke(spellNumber);
    }
    
}
