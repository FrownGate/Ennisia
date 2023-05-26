using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidInstinct : Skill
{
    private void Awake()
    {
        fileName = "DruidInstinct";
    }
    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        float MaxHpBuff = player.Damage * 1.5f;
        player.MaxHp += MaxHpBuff;
    }

    public override void PassiveAfterAttack(Entity target, Entity player, int turn, float damage)
    {
        healingModifier = damage * 0.05f;
    }

    //add revenge : after receiving dmg, give a shield of 5% of max hp 
}
