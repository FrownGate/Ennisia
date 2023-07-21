using System.Collections.Generic;
using UnityEngine;

public class QuakingSuppression : DamageSkill
{
    private float _stunPerc = 0.7f;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float stunLuck = Random.Range(0, 1);
        foreach (Entity target in targets)
        {
            if (stunLuck <= _stunPerc)
            {

                target.ApplyEffect(new Stun(1));
            }
            target.ApplyEffect(new Silence(2));
        }
        Cooldown = Data.MaxCooldown;

        return 0;
    }
}