using System.Collections.Generic;
using static Stat<float>;

public class GustofPrecision : Skill
{
    ModifierID id;
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        id = player.Stats[Item.AttributeStat.CritRate].AddModifier(AddCritRate);
    }

    float AddCritRate(float input)
    {
        return input + 25;
    }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.CritRate].RemoveModifier(id);
    }

}
