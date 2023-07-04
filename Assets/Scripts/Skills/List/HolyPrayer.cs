using System.Collections.Generic;

public class HolyPrayer : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        //attacksBuff

        caster.CurrentHp += caster.Stats[Attribute.HP].Value * 10 / 100;
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}