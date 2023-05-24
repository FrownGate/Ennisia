using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private float healingAmount;
    private float damageAmount;
    private float shieldAmount;
    private float penDef;
    private string description;
    private string skillName;
    private bool isAfter;
    private bool use;
    Texture2D texture;



    private void Use(target, player)
    {
        damageAmount = damageAmount * (1 + target.hp / targer.maxHp * 100);

    }
   
}
