using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalCrash : Skill
{


    private void Start()
    {
        fileName = "FatalCrash";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = data.damageAmount *  ((targets[0].currentHp + 100) / targets[0].maxHp); //HUGO TO BALANCE -> make excel
        targets[0].TakeDamage(damage);
        cd = data.maxCooldown;
        return damage;
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.currentHp += 80/100 * damage;
    }
}

