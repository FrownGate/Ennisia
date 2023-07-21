using System.Collections.Generic;

public class Aftermath : PassiveSkill
{
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            if (target.HasAlteration())
            {
                caster.AtkBarPercentage += 70;
            }
        }
        
    }
}
