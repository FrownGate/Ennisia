using System.Collections.Generic;

public class ShieldBump : DamageSkill
{
    float additionalDamage;
    public override float Use(List<Entity> targets, Entity caster, int turn)
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

    public override float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            foreach (Effect effect in target.Effects)
            {
                if (effect.GetType() == typeof(DefenseBuff))
                {
                    additionalDamage = effect.GetType() == typeof(DefenseBuff) ? damage * (Data.BuffAmount/100) : 0;
                }
            }
            target.TakeDamage(additionalDamage);
        }
        return additionalDamage;
    }
}