using System.Collections.Generic;

public class HiddenGift : PassiveSkill
{
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        if (turn % 3 == 0)
        {
            if (caster.HasBuff())
            {
                caster.AtkBarPercentage += 50;
            }
        } 
    }
}
