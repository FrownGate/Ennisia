using System.Collections.Generic;

public class TidalExecution : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (Entity target in targets)
        {
            float ratioHp = (target.CurrentHp / target.Stats[Attribute.HP].Value);

            if (ratioHp <= 0.05f)
            {
                //TODO -> Execute
                target.TakeDamage(5000000);
            }

            float damage = DamageCalculation(target,caster);
            float missingHp = 1 - ratioHp;
            damage = damage + (missingHp * 2 * damage);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}