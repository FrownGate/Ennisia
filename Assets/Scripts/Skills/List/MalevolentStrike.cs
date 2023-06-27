using System.Collections.Generic;

public class MalevolentStrike : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {
        // if atk buff, penetrate defense by 10%
    }
}