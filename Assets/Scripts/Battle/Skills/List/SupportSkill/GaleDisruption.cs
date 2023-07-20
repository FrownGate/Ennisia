using System.Collections.Generic;

public class GaleDisruption : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (var target in targets)
        {
            target.Strip();
        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}