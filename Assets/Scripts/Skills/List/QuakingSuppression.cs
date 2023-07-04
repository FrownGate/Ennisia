using System.Collections.Generic;
using UnityEngine;

public class QuakingSuppression : DamageSkill
{
    private float _stunPerc = 0.7f;
    private int _silenceTurn;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float stunLuck = Random.Range(0, 1);

        if (stunLuck > _stunPerc)
        {
            //TODO -> Stun 
        }
        // Silence

        return 0;
    }
}