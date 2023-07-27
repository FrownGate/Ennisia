using System.Collections.Generic;

public class DemonicStrike : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            float damage = DamageCalculation(target, caster);
            target.TakeDamage(damage);
            TotalDamage += damage;
            target.ApplyEffect(new SupportSilence());
        }
        Cooldown = Data.MaxCooldown;
        return TotalDamage;
    }
}