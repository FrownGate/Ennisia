using System.Collections.Generic;

public class FightUntilTheEnd : Skill
{
    ModifierID id;
    public float PhysicalBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.PhysicalDamages].AddModifier(PhysicalBuff);
    }
    float PhysicalBuff(float input)
    {
        return input * (1+PhysicalBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.PhysicalDamages].RemoveModifier(id);
    }
}
