using System.Collections.Generic;

public class ShieldBump : DamageSkill
{
    float additionalDamage;
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = Data.DamageRatio;
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    public override float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            foreach (Effect effect in target.Effects)
            {
                if (effect.GetType() == typeof(DefenseBuff))
                {
                    additionalDamage = effect.GetType() == typeof(DefenseBuff) ? damage / 2 : 0;
                }
            }
            target.TakeDamage(additionalDamage);
        }
        return additionalDamage;
    }
}