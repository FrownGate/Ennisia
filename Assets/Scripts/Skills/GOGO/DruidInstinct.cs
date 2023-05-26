using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidInstinct : Skill
{
    private void Awake()
    {
        fileName = "DruidInstinct";
    }
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float MaxHpBuff = player.Attack * 1.5f;
        player.MaxHp += MaxHpBuff;
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        healingModifier = damage * 0.05f;
    }

    //add revenge : after receiving dmg, give a shield of 5% of max hp 
}
