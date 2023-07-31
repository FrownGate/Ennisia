using System.Collections.Generic;

public class ZephyrsFury : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        caster.ApplyEffect(new CritDmgBuff(3));
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}