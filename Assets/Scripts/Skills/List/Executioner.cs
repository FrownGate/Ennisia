using System.Collections.Generic;


public class Executioner : Skill
{
    ModifierID id;
    public float criticalDmgBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.CritDmg].AddModifier(CritDmgBuff);
    }
    float CritDmgBuff(float input)
    {
        return input + criticalDmgBuffRatio;
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.CritDmg].RemoveModifier(id);
    }
}
