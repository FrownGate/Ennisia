using System.Collections.Generic;

public class AllIn : Skill
{
    public float defBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        player.Speed = 10000; // change to take only first turn
        float attackBuff = player.PhysDef * (defBaseRatio + StatUpgrade1 * Level) + player.MagicDef * (defBaseRatio + StatUpgrade1 * Level);
        player.Attack = attackBuff;
    }
}