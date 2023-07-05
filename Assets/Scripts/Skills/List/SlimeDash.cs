using System.Collections.Generic;

public class SlimeDash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;

        // Add 10% chance silence debuff
    }
}