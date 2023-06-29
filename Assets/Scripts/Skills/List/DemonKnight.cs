using System.Collections.Generic;

public class DemonKnight : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {
        // if target has demonic mark, add 2 more for 1 turn
    }
}