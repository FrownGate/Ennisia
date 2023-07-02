using System.Collections.Generic;

public class SpinAttack : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // Add damage amount and is physical
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        return damage;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.AtkBarPercentage += 5;
    }
}