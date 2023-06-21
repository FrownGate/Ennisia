using System.Collections.Generic;


public class DemonAndHuman : Skill
{
    ModifierID id;
    ModifierID id2;
    public float magicalBuffRatio;
    public float PhysicalBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.MagicalDamages].AddModifier(MagicalDefBuff);
        id2 = player.Stats[Item.AttributeStat.PhysicalDamages].AddModifier(PhysicalDefBuff);
    }
    float MagicalDefBuff(float input)
    {
        return input * (1 + magicalBuffRatio);
    }
    float PhysicalDefBuff(float input)
    {
        return input * (1 + PhysicalBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.MagicalDamages].RemoveModifier(id);
        player.Stats[Item.AttributeStat.PhysicalDamages].RemoveModifier(id2);
    }
}
