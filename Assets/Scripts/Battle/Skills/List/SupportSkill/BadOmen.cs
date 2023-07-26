using System.Collections.Generic;

public class BadOmen : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new DefenseBuff());
        caster.Shield += (caster.Stats[Attribute.HP].Value * Data.ShieldAmount);
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}