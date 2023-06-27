using System.Collections.Generic;

public class DemonicArcher : PassiveSkill
{
    List<ModifierID> id;
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        id.Add(player.Stats[Item.AttributeStat.MagicalDefense].AddModifier(DefenseBuf));
        id.Add(player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(DefenseBuf));
    }
    float DefenseBuf(float input)
    {
        return (float)input * 1.20f;
    }
}