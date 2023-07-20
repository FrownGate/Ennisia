using System.Collections.Generic;

public class SpinAttack : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;          
        }
        caster.AtkBarPercentage += (caster.AtkBarPercentage*((int)Data.BuffAmount/100));
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}