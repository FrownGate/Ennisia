using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : DamageSkill
{
    //support skill

    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            int debuffLuck = Random.Range(0, 100);
            if (debuffLuck <= 80)
            {
                target.ApplyEffect(new BreakDefense());
            }
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}