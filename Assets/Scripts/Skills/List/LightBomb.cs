using System.Collections.Generic;

public class LightBomb : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float TotalDamage = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            float damage = Data.DamageAmount;
            targets[i].TakeDamage(damage);
            TotalDamage += damage;
        }

        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}