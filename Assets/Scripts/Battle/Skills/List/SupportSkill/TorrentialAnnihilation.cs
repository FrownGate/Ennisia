using System.Collections.Generic;

public class TorrentialAnnihilation : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {

        foreach (var target in targets)
        {

            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);

            TotalDamage += damage;
            if (caster.Stats[Attribute.PhysicalDamages].Value > target.Stats[Attribute.PhysicalDamages].Value)
            {
                caster.Cleanse();
            }
        }
        Cooldown = Data.MaxCooldown;

        return TotalDamage;
    }

    public override float AdditionalDamage(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (var target in targets)
        {
            target.DefIgnored += 100;
            target.TakeDamage(damage * Data.IgnoreDef);
            TotalDamage += damage * Data.IgnoreDef;
            target.DefIgnored -= 100;
        }
        return TotalDamage;
    }
}