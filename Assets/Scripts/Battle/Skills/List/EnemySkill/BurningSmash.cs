using System.Collections.Generic;
using System;

public class BurningSmash : DamageSkill
{
    private int _percentChance = 20;

    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            int _randomNumber = new Random().Next(1, 100);

            if (_randomNumber <= _percentChance)
            {
                target.ApplyEffect(new BreakDefense());
            }
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}