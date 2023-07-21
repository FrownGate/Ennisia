using System.Collections.Generic;

public class GushingSpores : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            target.ApplyEffect(new BreakAttack());
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}