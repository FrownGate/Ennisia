using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : Skill
{
    /// TO DO -> add defense debuff for x turns
    
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        DamageModifier = player.PhysAtk * StatUpgrade1 * Level;

        float debuffLuck = Random.Range(0, 1);

        if (debuffLuck <= 0.8)
        {
            //debuff defense by 70%
        }

        return 0;
    }
}