using System.Collections.Generic;
using UnityEngine;

public class AquaParalysis : Skill
{
    //TODO -> When the player is attacked, has a 15% chance to stun the enemy.
    public override void UseIfAttacked(List<Entity> targets, Entity player, int turn, float damageTaken)
    {
        float stunLuck = Random.Range(0, 1);

        if (stunLuck <= 0.15)
        {
            //Stun
        }

    }
}
