using System.Collections.Generic;

public class DemonicStrike : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return 0;
    }

    // add support silence debuff
}