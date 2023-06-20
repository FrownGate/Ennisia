using System.Collections.Generic;
using static Stat<float>;

public class AllIn : Skill
{
    public float defBaseRatio;
    ModifierID id;
    float attackBuff;


    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        player.atkBarPercentage = 100;
        attackBuff = player.Stats[Item.AttributeStat.Defense].Value * (defBaseRatio + StatUpgrade1 * Level) + player.Stats[Item.AttributeStat.MagicalDefense].Value * (defBaseRatio + StatUpgrade1 * Level);
        id = player.Stats[Item.AttributeStat.Attack].AddModifier(AttackBuff);
    }

    float AttackBuff(float input)
    {   
        return input + attackBuff;
    }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Attack].RemoveModifier(id);
    }
}
