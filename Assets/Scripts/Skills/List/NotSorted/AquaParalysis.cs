using System.Collections.Generic;
using UnityEngine;

public class AquaParalysis : PassiveSkill
{
    //TODO -> When the player is attacked, has a 15% chance to stun the enemy.
    public override void UseIfAttacked(List<Entity> targets , Entity caster , Entity player, int turn, float damageTaken) //caster et player ?
    {
        float stunLuck = Random.Range(0, 1);

        if (stunLuck <= 0.15)
        {
            targets[0].ApplyEffect(new Stun());
        }

    }
}