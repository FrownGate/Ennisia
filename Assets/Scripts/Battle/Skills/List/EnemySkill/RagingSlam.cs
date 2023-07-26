using System.Collections.Generic;

public class RagingSlam : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        caster.ApplyEffect(new AttackBuff());
        Cooldown = Data.MaxCooldown;
        return TotalDamage;

    }

 
}