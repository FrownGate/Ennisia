using System.Collections.Generic;


public class WarriorWill : Skill
{
    ModifierID id;
    ModifierID id2;
    public float PhysicalDmgBuffRatio;
    public float PhysicalDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.PhysicalDamages].AddModifier(PhysicalBuff);
        id2 = player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(PhysicalDefBuff);
    }
    float PhysicalBuff(float input)
    {
        return input * (1 + PhysicalDmgBuffRatio);
    }
    float PhysicalDefBuff(float input)
    {
        return input * (1 + PhysicalDefBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.PhysicalDamages].RemoveModifier(id);
        player.Stats[Item.AttributeStat.PhysicalDefense].RemoveModifier(id2);
    }
}
