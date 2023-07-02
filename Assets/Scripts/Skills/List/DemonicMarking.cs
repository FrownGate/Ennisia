using System.Collections.Generic;
using System;

public class DemonicMarking : DamageSkill
{
    private int[] _percentChances = {100, 50, 50};

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // add damage amount and is magic dmage
        targets[0].TakeDamage(damage);

        for (int i = 0; i < _percentChances.Length; i++)
        {
            int randomNumber = new Random().Next(1, 100);

            if (_percentChances[i] >= randomNumber)
            {
                //add defense debuff, Crit Rate Buff, Crit Dmg Buff
            }
        }

        Cooldown = Data.MaxCooldown;
        return damage;
    }
}