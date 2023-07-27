using System.Collections.Generic;

public class InfernalResilience : ProtectionSkill
{
    public override float Use(List<Entity> target, Entity caster, int turn, List<Entity> allies)
    {
        ShieldModifier = ((Data.ShieldAmount / 100) + StatUpgrade1 * Level) * (caster.Stats[Attribute.HP].Value - caster.CurrentHp);
        caster.Shield += ShieldModifier;
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}