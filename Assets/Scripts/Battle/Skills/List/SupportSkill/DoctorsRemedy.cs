using System.Collections.Generic;

public class DoctorsRemedy : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        caster.ApplyEffect(new Immunity(3));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}
