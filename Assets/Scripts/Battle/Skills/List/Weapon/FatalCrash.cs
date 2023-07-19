using System.Collections.Generic;

public class FatalCrash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float missingHpRatio = (target.CurrentHp / target.Stats[Attribute.HP].Value);
            if (missingHpRatio < 0.3f) missingHpRatio = 0.3f;
            float damage = DamageCalculation(target, caster);
            damage *= (1 / missingHpRatio);
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