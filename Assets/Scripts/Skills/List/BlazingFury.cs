using System.Collections.Generic;

public class BlazingFury : BuffSkill
{
    private int _increaseAttTurn;

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        if (turn <= _increaseAttTurn)
        {
            //TODO -> increase damage  
        }
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        _increaseAttTurn = turn + 3;
        return 0;
    }
}
