using System.Collections.Generic;

public class FlamePurification : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.Cleanse();
        return 0;
    }
}