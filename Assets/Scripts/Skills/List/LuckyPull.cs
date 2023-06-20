using System.Collections.Generic;


public class LuckyPull : Skill
{
    ModifierID id;
    public float criticalRateBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.CritRate].AddModifier(CritRateBuff);
    }
    float CritRateBuff(float input)
    {
        return input + criticalRateBuffRatio;
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.CritRate].RemoveModifier(id);
    }
}
