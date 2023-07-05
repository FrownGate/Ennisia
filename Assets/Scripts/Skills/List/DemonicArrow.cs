using System.Collections.Generic;
using System;

public class DemonicArrow : DamageSkill
{
    private int _percentChance = 40;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        int randomNumber = new Random().Next(1, 100);

        if (_percentChance >= randomNumber)
        {
            caster.ApplyEffect(new DefenseBuff());
        }

        Cooldown = Data.MaxCooldown;
        return 0;
    }
}