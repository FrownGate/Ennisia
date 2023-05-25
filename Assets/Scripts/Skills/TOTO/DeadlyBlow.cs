using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyBlow : Skill
{


    private void Start()
    {
        fileName = "DeadlyBlow";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = data.damageAmount;
        targets[0].TakeDamage(damage);
        
        return damage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        if (targets[0].IsDead)
        {
            cd = 0;
        }
    }
}

