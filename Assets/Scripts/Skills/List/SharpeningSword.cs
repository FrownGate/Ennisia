using System.Collections.Generic;

public class SharpeningSword : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new AttackBuff());
        return 0;
    }

    // need the 3 turns change of buff
}