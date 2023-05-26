using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBomb : Skill
{


    private void Awake()
    {
        fileName = "LightBomb";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float TotalDamage = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            float damage = data.damageAmount;
            targets[i].TakeDamage(damage);
            TotalDamage += damage;
        }
        cd = data.maxCooldown;
        return TotalDamage;
    }
}

