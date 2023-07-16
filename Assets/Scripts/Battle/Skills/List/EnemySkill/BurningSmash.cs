using System.Collections.Generic;
using System;

public class BurningSmash : DamageSkill
{
    private int _percentChance = 20;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        int randomNumber = new Random().Next(1, 100);

        if (_percentChance >= randomNumber)
        {
            targets[0].ApplyEffect(new BreakDefense());
        }

        Cooldown = Data.MaxCooldown;
        return damage;
    }
}