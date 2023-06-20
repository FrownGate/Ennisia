using System.Collections.Generic;
using static Stat<float>;
public class InTheShadow : Skill
{
    ModifierID id;
    public float DefIgnoredBaseRatio;
    public float DefIgnoredBuff;
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        DefIgnoredBuff = DefIgnoredBaseRatio + (StatUpgrade1 * Level);
        id = targets[0].Stats[Item.AttributeStat.DefIgnoref].AddModifier(IgnoreDef);
    }

    float IgnoreDef(float input)
    {
        return input + 40;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        //if debuffed
        player.atkBarPercentage = 100;
    }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        targets[0].Stats[Item.AttributeStat.DefIgnoref].RemoveModifier(id);
    }
    // to do : if enemy is debuff, #% chance to play again
}