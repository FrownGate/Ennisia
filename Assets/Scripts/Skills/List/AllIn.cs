using System.Collections.Generic;

public class AllIn : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        player.Speed = 10000;
        float attackBuff = player.PhysDef * 0.5f + player.MagicDef * 0.5f;
        player.Attack = attackBuff;
    }
}