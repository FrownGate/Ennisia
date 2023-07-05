using System.Collections.Generic;

public class DemonicStrike : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = DamageCalculation(targets[0], caster);
        targets[0].TakeDamage(damage);
        targets[0].ApplyEffect(new SupportSilence());
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}