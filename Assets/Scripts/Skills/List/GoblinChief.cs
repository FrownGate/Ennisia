using System.Collections.Generic;

public class GoblinChief : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {
        // if target is stun, penetrate defense by 50%
    }
}