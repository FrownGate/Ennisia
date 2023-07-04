using System.Collections.Generic;

public class BonesBreaker : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        targets[0].ApplyEffect(new BreakDefense());
        Cooldown = Data.MaxCooldown;
        return damage;
    }
}