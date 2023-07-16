using System.Collections.Generic;

public class HolyPrayer : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        //attacksBuff

        caster.Heal(caster.Stats[Attribute.HP].Value * 10 / 100);
        caster.ApplyEffect(new AttackBuff());
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}