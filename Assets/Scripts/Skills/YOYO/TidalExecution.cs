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
        float percHPRemaining = targets[0].CurrentHp / targets[0].MaxHp;
        if (percHPRemaining <= 0.05f)
        {
            //TODO -> Execute
            targets[0].TakeDamage(targets[0].MaxHp);
        }
        float missingHealth = targets[0].MaxHp - targets[0].CurrentHp;
        float damage = data.damageAmount * missingHealth;
        targets[0].TakeDamage(damage);

        return damage;
    }
}