using System.Collections.Generic;

public class DemonicArcher : PassiveSkill
{
    List<ModifierID> id;
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        id[0] = player.Stats[Item.AttributeStat.MagicalDefense].AddModifier(DefenseBuf);
        id[1] = player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(DefenseBuf);
    }
    float DefenseBuf(float input)
    {
        return (float)input * 1.20f;
    }
}