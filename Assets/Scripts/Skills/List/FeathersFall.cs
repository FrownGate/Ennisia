using System.Collections.Generic;

public class FeathersFall : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach(var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}