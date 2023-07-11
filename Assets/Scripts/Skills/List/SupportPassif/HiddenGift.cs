using System.Collections.Generic;

public class HiddenGift : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        if (caster.HasBuff())
        {
            caster.AtkBarPercentage += 50;
        }
    }
}
