using System.Collections.Generic;

public class LightBomb : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float _totalDamage = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            float damage = Data.DamageAmount;
            targets[i].TakeDamage(damage);
            _totalDamage += damage;
        }

        Cooldown = Data.MaxCooldown;
        return _totalDamage;
    }
}