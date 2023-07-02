using System.Collections.Generic;

public class FeathersFall : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float totalDamage = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            float damage = Data.DamageAmount;
            targets[i].TakeDamage(damage);
            totalDamage += damage;
        }

        Cooldown = Data.MaxCooldown;
        return totalDamage;
    }
}