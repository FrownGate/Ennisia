using System.Collections.Generic;


public class AccuracyDevice : Skill
{
    ModifierID id;
    ModifierID id2;
    ModifierID id3;
    public float CritRateBuffRatio;
    public float CritDmgBuffRatio;
    public float AttackBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.CritRate].AddModifier(CritRateBuff);
        id2 = player.Stats[Item.AttributeStat.CritDmg].AddModifier(CritDmgBuff);
        id3 = player.Stats[Item.AttributeStat.Attack].AddModifier(AttackBuff);
    }
    float CritRateBuff(float input)
    {
        return input * (1 + CritRateBuffRatio);
    }
    float CritDmgBuff(float input)
    {
        return input * (1 + CritDmgBuffRatio);
    }
    float AttackBuff(float input)
    {
        return input * (1 + AttackBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.CritRate].RemoveModifier(id);
        player.Stats[Item.AttributeStat.CritDmg].RemoveModifier(id2);
        player.Stats[Item.AttributeStat.Attack].RemoveModifier(id3);

    }
}
