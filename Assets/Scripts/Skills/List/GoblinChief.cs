using System.Collections.Generic;

public class GoblinChief : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        // if target is stun, penetrate defense by 50%
    }
}