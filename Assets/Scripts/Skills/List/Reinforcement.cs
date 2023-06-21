using System.Collections.Generic;

public class Reinforcement : PassiveSkill
{
    public float defBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float defRatio = defBaseRatio + StatUpgrade1 * Level;
        float PhdefBuff = player.Stats[Item.AttributeStat.Defense].Value * defRatio;
        float MdefBuff = player.Stats[Item.AttributeStat.MagicalDefense].Value * defRatio;

    }
}