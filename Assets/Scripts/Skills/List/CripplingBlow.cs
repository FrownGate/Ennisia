using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : DamageSkill
{
    /// TO DO -> add defense debuff for x turns

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        DamageModifier = (50 / 100 * player.Stats[Item.AttributeStat.PhysicalDamages].Value) * StatUpgrade1 * Level;

        float debuffLuck = Random.Range(0, 1);

        if (debuffLuck <= 0.8)
        {
            //debuff defense by 70%SSS
        }

        return 0;
    }
}