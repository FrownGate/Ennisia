using System.Collections.Generic;
using System;

public class GolemSmash : DamageSkill
{
    private int _percentChance = 10;
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        int randomNumber = new Random().Next(1, 100);
        if (_percentChance >= randomNumber)
        {
            targets[0].ApplyEffect(new Stun());
        }
        Cooldown = Data.MaxCooldown;
        return damage * 2;
    }
}