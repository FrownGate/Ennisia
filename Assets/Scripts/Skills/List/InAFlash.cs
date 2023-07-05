using System.Collections.Generic;

public class InAFlash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach(Entity target in targets)
        {
            target.DefIgnored += 50;
            float damage = DamageCalculation(target, caster);
            target.DefIgnored -= 50;
            TotalDamage += damage;
            target.TakeDamage(damage);

        }
        return TotalDamage;
    }
}