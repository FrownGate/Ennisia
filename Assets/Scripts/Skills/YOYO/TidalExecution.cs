using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TidalExecution : Skill
{

    private void Awake()
    {
        fileName = "TidalExecution";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float percHPRemaining = targets[0].currentHp / targets[0].maxHp;
        if (percHPRemaining <= 0.05f)
        {
            //TODO -> Execute
            targets[0].TakeDamage(targets[0].maxHp);
        }
        float missingHealth = targets[0].maxHp - targets[0].currentHp;
        float damage = data.damageAmount * missingHealth;
        targets[0].TakeDamage(damage);

        return damage;
    }
}