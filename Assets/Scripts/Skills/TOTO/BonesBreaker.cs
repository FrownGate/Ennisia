using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesBreaker : Skill
{


    private void Awake()
    {
        fileName = "BonesBreaker";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = data.damageAmount;
        targets[0].TakeDamage(damage);
        //breakdef
        cd = data.maxCooldown;
        return damage;
    }
}

