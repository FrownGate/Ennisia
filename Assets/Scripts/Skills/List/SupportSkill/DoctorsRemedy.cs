using System.Collections.Generic;

public class DoctorsRemedy : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        //TODO add immunity buff
        //caster.ApplyEffect(new ImmunityBuff(3));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}
