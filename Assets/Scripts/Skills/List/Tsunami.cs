using System.Collections.Generic;

public class Tsunami : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = 0; // add damage amount and is magic dmage
        targets[0].TakeDamage(damage);
        targets[0].AtkBarPercentage -= 30;
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}