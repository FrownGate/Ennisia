using System.Collections.Generic;

public class StickyRoots : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            target.AtkBarPercentage -= 50;

        }
        Cooldown = Data.MaxCooldown;
        return 0;
    }
}