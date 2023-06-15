using System.Collections.Generic;

public class RefreshingTideguard : Skill
{
    //TODO -> Gives a shield to the player, scales with max Hp, for 2 turns.
    private int _shieldTurn;

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        if (turn <= _shieldTurn)
        {

            //TODO -> increase damage  
        }
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        _shieldTurn = turn + 2;
        return 0;
    }

}
