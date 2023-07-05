using System.Collections.Generic;

public class ShieldBump : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = Data.DamageRatio;
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    public override float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage)
    {
        if targetHasDefBuff
        foreach(Entity target in targets)
            {
                foreach(Effect effect in target.Effects)
                {

                }
                float additionalDamage = target.Effects.Count ? damage / 2;
            }
       
        target.TakeDamage(additionalDamage);
        return additionalDamage
    }
}