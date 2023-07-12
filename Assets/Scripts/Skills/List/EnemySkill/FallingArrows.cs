using System.Collections.Generic;
using System;

public class FallingArrows : DamageSkill
{
    private int[] _percentChances = { 50, 50 };
    private Effect[] _effects = { new CritRateBuff(), new CritDmgBuff() };
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
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