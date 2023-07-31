using System.Collections.Generic;
using UnityEngine;

public class AquaParalysis : PassiveSkill
{
    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken,
        List<Entity> allies) //caster et player ?
    {
            float stunLuck = Random.Range(0, 100);

            if (stunLuck <= 15)
            {
                caster.ApplyEffect(new Stun());
            }
    }
}