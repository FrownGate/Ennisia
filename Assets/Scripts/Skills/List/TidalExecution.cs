using System.Collections.Generic;

public class TidalExecution : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float percHPRemaining = target.CurrentHp / target.Stats[Attribute.HP].Value;

            if (percHPRemaining <= 0.05f)
            {
                //TODO -> Execute
                target.TakeDamage(5000000);
            }



            float missingHealth = target.Stats[Attribute.HP].Value - target.CurrentHp;
            float damage = DamageCalculation(target,caster) * missingHealth;
            TotalDamage+= damage;
            target.TakeDamage(damage);
        }

        return TotalDamage;
    }
}