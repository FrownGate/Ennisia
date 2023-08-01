using System.Collections.Generic;

public class FlamePurification : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        caster.RemoveAlterations();
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}