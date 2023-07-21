using System.Collections.Generic;
using System;

public class ChiefsOrders : DamageSkill
{
    private readonly int _percentChance = 25;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            int randomNumber = new Random().Next(1, 100);
            if (randomNumber <= _percentChance)
            {
                target.ApplyEffect(new Stun());
            }
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}