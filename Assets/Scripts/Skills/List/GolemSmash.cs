using System.Collections.Generic;

public class GolemSmash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // Add damage amount and is physical damage is x2
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;

        // add 10% chance to stun
    }
}