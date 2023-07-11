using System.Collections.Generic;

public class FatalCrash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            float damage = Data.DamageRatio * (1 + (target.CurrentHp / target.Stats[Attribute.HP].Value));
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        caster.CurrentHp += 0.8f * damage;
    }
}