using System.Collections.Generic;

public class GalebladeStrike : Skill
{
    //TODO -> Give an attack buff for 3 turns before attacking an enemy, damage scales with att.
    private int _increaseAttTurn;

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        if (turn <= _increaseAttTurn)
        {
            //TODO -> give att Buff 
        }
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        _increaseAttTurn = turn + 3;
        return 0;
    }

}
