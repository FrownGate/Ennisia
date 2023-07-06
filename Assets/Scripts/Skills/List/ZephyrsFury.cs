using System.Collections.Generic;

public class ZephyrsFury : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new CritDmgBuff());
        return 0;
    }
}