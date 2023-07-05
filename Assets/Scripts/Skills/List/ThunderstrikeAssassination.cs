using System.Collections.Generic;

public class ThunderstrikeAssassination : DamageSkill
{
    //TODO -> finish skill
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            target.DefIgnored += 100;
            float damage = DamageCalculation(target, caster);
            target.DefIgnored += 100;
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}