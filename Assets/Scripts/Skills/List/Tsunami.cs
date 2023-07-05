using System.Collections.Generic;

public class Tsunami : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        targets[0].AtkBarPercentage -= 30;
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}