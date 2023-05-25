using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalCrash : Skill
{


    private void Awake()
    {
        fileName = "TidalCrash";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = data.damageAmount;
        targets[0].TakeDamage(damage);
        return damage;
    }
}

