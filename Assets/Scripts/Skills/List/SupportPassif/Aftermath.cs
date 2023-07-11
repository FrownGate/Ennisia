using System.Collections.Generic;

public class Aftermath : PassiveSkill
{
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        if (caster.HasAlteration())
        {
            caster.AtkBarPercentage += 70;
        }
    }
}
