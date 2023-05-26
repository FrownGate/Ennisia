using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBump : Skill
{


    private void Awake()
    {
        fileName = "ShieldBump";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = data.damageAmount;
        targets[0].TakeDamage(damage);
        cd = data.maxCooldown;
        return damage;
    }   

    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage)
    {

        //if targetHasDefBuff
        //float additionalDamage = damage / 2;
        //target.TakeDamage(additionalDamage);
        //return additionalDamage
        return 0;
    }
}

