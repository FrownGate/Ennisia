using System.Collections.Generic;

public class RagingSlam : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new AttackBuff());

        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    // add atk buff
}