using System.Collections.Generic;

public class GaleDisruption : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (var target in targets)
        {
            target.RemoveBuffs();
        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}