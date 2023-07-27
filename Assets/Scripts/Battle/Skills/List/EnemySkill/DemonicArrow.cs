using System.Collections.Generic;
using System;

public class DemonicArrow : BuffSkill
{
    private int _percentChance = 40;

    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
           
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