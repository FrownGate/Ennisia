using System.Collections.Generic;

public class ChiefsOrders : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // Add damage amount and is physical
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        targets[0].ApplyEffect(new Stun());
        return damage;

        // add 25% chance to stun
    }
}