using System.Collections.Generic;
using static Stat<float>;

public class ZephyrSwiftness : Skill
{
    ModifierID id;
    //TODO -> Increase base speed by 20%.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        id = player.Stats[Item.AttributeStat.Speed].AddModifier(IncreaseSpeed);
    }
    float IncreaseSpeed(float input) { return input + (int)(input * 20 / 100); }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Speed].RemoveModifier(id);
    }
}
