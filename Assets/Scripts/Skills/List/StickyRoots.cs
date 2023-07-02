using System.Collections.Generic;

public class StickyRoots : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        targets[0].AtkBarPercentage -= 50;
        Cooldown = Data.MaxCooldown;
        return 0;
    }

    
}