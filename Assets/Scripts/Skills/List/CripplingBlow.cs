using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : Skill
{
    /// TO DO -> add defense debuff for x turns

    private void Awake()
    {
        FileName = "CripplingBlow";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = player.PhysAtk * Data.damageAmount/100;

        float debuffLuck = Random.Range(0, 1);

        if (debuffLuck >= 0.8)
        {
            //debuff defense by 70%
        }

        return damage;
    }
}