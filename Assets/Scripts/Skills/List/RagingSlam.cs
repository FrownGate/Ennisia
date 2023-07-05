using System.Collections.Generic;

public class RagingSlam : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        caster.ApplyEffect(new AttackBuff());
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}