using System.Collections.Generic;

public class ImpregnableDefense : Skill
{
    private float _defenseAdded;
    private float _defenseToAdd;

    private void Awake()
    {
        FileName = "ImpregnableDefense";
        _defenseAdded= 0;
    }

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        //_defenseToAdd = player.Def * 5/100
    }

    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {

        _defenseAdded = turn < 4 ? _defenseToAdd * turn : _defenseAdded;
        //need to add afterBattleFunction to take off stats
    }
}