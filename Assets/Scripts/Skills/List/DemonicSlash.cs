using System.Collections.Generic;
using System;

public class DemonicSlash : DamageSkill
{
    private int _randomNumber;
    private int _percentChance = 10;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // add damage amount and is magic dmage
        targets[0].TakeDamage(damage);
        _randomNumber = new Random().Next(1, 100);

        if (_percentChance >= _randomNumber)
        {
            //add demonic mark
        }

        Cooldown = Data.MaxCooldown;
        return damage;
    }
}