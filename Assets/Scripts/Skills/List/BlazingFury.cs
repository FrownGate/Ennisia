using System.Collections.Generic;

public class BlazingFury : Skill
{
    private int _increaseAttTurn;

    private void Awake()
    {
        FileName = "BlazingFury";
    }

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
