using System.Collections.Generic;

public class GushingSpores : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        targets[0].ApplyEffect(new BreakAttack());
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}