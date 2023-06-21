using System.Collections.Generic;


public class ArmorOfTheDeads : Skill
{
    ModifierID id;
    ModifierID id2;
    public float magicalDefBuffRatio;
    public float PhysicalDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.MagicalDamages].AddModifier(MagicalDefBuff);
        id2 = player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(PhysicalDefBuff);
    }
    float MagicalDefBuff(float input)
    {
        return input * (1 + magicalDefBuffRatio);
    }
    float PhysicalDefBuff(float input)
    {
        return input * (1 + PhysicalDefBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.MagicalDamages].RemoveModifier(id);
        player.Stats[Item.AttributeStat.PhysicalDefense].RemoveModifier(id2);
    }
}
