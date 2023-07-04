using System.Collections.Generic;

public class How : BuffSkill
{
    
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        for (int i = 1; i < targets.Count; i++)
        {
            // add atk Buff for 2 turns
        }

        Cooldown = Data.MaxCooldown;
        return 0;
    }
}