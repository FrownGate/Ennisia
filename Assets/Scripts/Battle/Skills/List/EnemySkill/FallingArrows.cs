using System.Collections.Generic;
using System;

public class FallingArrows : DamageSkill
{
    private int _percentChances = 50;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }

        int randomNumber = new Random().Next(1, 100);

        if (randomNumber <= _percentChances)
        {
            caster.ApplyEffect(new CritRateBuff());
            caster.ApplyEffect(new CritDmgBuff());
        }

        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}