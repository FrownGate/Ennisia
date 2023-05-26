using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodContract : Skill
{


    private void Awake()
    {
        fileName = "BloodContract";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float TotalDamage = 0;
       for(int i = 0; i < targets.Count; i++)
        {
            float damage = data.damageAmount;
            targets[i].TakeDamage(damage);
            TotalDamage += damage;
        }
        return TotalDamage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.currentHp += damage * 30 / 100;
    }
}

