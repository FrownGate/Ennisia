using System.Collections.Generic;
using System;

public class DemonicMarking : DamageSkill
{
    private int[] _percentChances = { 50, 50};
    private Effect[] _effects = { new CritRateBuff(), new CritDmgBuff() };

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // add damage amount and is magic dmage
        targets[0].TakeDamage(damage);
        targets[0].ApplyEffect(new DefenseBuff());
        for (int i = 0; i < _percentChances.Length; i++)
        {
            int randomNumber = new Random().Next(1, 100);

            if (_percentChances[i] >= randomNumber)
            {
                caster.ApplyEffect(_effects[i]);
            }
        }

        Cooldown = Data.MaxCooldown;
        return damage;
    }
}