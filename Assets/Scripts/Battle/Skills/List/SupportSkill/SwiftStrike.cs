using System.Collections.Generic;

public class SwiftStrike : DamageSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            target.DefIgnored += 10;
        }
    }
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
        }
        return TotalDamage;
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            target.DefIgnored -= 10;
        }
        caster.AtkBarPercentage += 50;
    }
}