using System.Collections.Generic;

public class DemonicDash : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // Add damage amount and is magic
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;

        // Add 10% chance demonic mark debuff
    }
}