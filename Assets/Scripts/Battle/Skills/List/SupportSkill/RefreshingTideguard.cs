using System.Collections.Generic;

public class RefreshingTideguard : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.Shield += caster.Stats[Attribute.HP].Value * (Data.ShieldAmount/100);
        Cooldown = Data.MaxCooldown;
        return 0;
    }

}