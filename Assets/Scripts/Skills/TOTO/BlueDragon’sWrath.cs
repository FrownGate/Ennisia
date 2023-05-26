using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLueDragonWraith : Skill
{


    private void Start()
    {
        fileName = "BlueDragonWraith";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = data.damageAmount;
        targets[0].TakeDamage(damage);
        cd = data.maxCooldown;
        return damage;
    }
}

