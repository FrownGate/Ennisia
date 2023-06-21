using System.Collections.Generic;


public class Herald : Skill
{
    ModifierID id;
    public float SpeedBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.Speed].AddModifier(SpeedBuff);
    }
    float SpeedBuff(float input)
    {
        return input * (1 + SpeedBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Speed].RemoveModifier(id);
    }
}
