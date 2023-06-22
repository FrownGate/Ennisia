using System.Collections.Generic;


public class IntegralArmour : Skill
{
    ModifierID id;
    public float PhysicalDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(PhysicalDefBuff);
    }
    float PhysicalDefBuff(float input)
    {
        return input * (1 + PhysicalDefBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.PhysicalDefense].RemoveModifier(id);
    }
}
