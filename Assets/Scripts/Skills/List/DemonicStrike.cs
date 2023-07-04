using System.Collections.Generic;

public class DemonicStrike : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = 0; // add damage amount and is physical dmage
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        targets[0].ApplyEffect(new SupportSilence());
        return 0;
    }

   
}