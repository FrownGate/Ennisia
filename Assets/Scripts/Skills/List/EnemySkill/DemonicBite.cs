using System.Collections.Generic;
using System;

public class DemonicBite : DamageSkill
{
    private int _percentChance = 50;
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        targets[0].ApplyEffect(new DemonicMark());
        int randomNumber = new Random().Next(1, 100);
        if (_percentChance >= randomNumber)
        {
            targets[0].ApplyEffect(new BreakDefense());
            caster.AtkBarPercentage = 100;
        }
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}