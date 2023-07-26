using System.Collections.Generic;

public class SwiftStrike : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            target.DefIgnored += 10;
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        caster.AtkBarPercentage += 50;
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            target.DefIgnored -= 10;
        }
    }
}