using System.Collections.Generic;

public class BloodContract : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }

    public override void SkillAfterDamage(List<Entity> targets, Entity caster, int turn, float damage,
        List<Entity> allies)
    {
        caster.Heal(damage * Data.HealingAmount / 100);
    }
}
