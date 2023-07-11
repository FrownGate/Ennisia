using System.Collections.Generic;

public class LightBomb : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float _totalDamage = 0;

        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target,caster);
            target.TakeDamage(damage);
            _totalDamage += damage;
        }

        Cooldown = Data.MaxCooldown;
        return _totalDamage;
    }
}