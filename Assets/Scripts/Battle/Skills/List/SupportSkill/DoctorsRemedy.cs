using System.Collections.Generic;

public class DoctorsRemedy : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new Immunity(3));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}
