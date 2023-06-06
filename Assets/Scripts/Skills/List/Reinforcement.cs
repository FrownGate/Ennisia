using System.Collections.Generic;

public class Reinforcement : Skill
{
    public float defBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float defRatio = defBaseRatio + StatUpgrade1 * Level;
        float PhdefBuff = player.PhysDef * defRatio;
        float MdefBuff = player.MagicDef * defRatio;
        player.MagicDef = MdefBuff;
        player.PhysDef = PhdefBuff;
    }
}