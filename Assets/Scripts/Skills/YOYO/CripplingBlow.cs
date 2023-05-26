using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : Skill
{

    ///TO DO -> replace player.damage for physical damage
    /// TO DO -> add defense debuff for x turns
    /// 


    private void Awake()
    {
        fileName = "CripplingBlow";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = player.Damage * data.damageAmount/100;

        float debuffLuck = Random.Range(0, 1);
        if (debuffLuck >= 0.8)
        {
            //debuff defense by 70%
        }
        return damage;
    }
}
