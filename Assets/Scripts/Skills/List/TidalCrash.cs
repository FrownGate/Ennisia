using System.Collections.Generic;

public class TidalCrash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        
        foreach(Entity target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        return TotalDamage;
    }
}