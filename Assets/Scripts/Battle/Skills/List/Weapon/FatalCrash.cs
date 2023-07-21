using System.Collections.Generic;

public class FatalCrash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float ratioHp = (target.CurrentHp / target.Stats[Attribute.HP].Value);
            float damage = DamageCalculation(target, caster);
            float missingHp = 1 - ratioHp;
            damage = damage + (missingHp * 2 * damage);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        caster.CurrentHp += (Data.HealingAmount/100) * damage;
    }
}