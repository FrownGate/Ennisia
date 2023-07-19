using System.Collections.Generic;

public class InAFlash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach(Entity target in targets)
        {
            target.DefIgnored += Data.IgnoreDef;
            float damage = DamageCalculation(target, caster);
            target.DefIgnored -= Data.IgnoreDef;
            TotalDamage += damage;
            target.TakeDamage(damage);
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}