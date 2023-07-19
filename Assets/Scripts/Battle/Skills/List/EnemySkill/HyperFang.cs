using System.Collections.Generic;
using System;
using System.Diagnostics;

public class HyperFang : DamageSkill
{
    private int _percentChance = 20;
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);

        int randomNumber = new Random().Next(1, 100);
        if (_percentChance >= randomNumber)
        {
            caster.ApplyEffect(new BreakDefense());
        }
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}