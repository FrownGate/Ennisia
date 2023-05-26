using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAFlash : Skill
{


    private void Awake()
    {
        fileName = "InAFlash";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //give 50% ignoreDef to Enemy
        float damage = data.damageAmount;
        targets[0].TakeDamage(damage);
        //Take it back
        cd = data.maxCooldown;
        return damage;
    }
}

