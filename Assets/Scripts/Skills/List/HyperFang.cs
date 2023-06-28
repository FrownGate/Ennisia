using System.Collections.Generic;

public class HyperFang : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // Add damage amount and is physical
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;

        // add the 20% chance defense break debuff
    }
}