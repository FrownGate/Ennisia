using System.Collections.Generic;

public class FallingArrows : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    // add 50% chance to give Crit rate and Crit Dmg buff
}