using System.Collections.Generic;
using static Stat<float>;

public class FlawlessTechnique : Skill
{
    ModifierID id;
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        id = player.Stats[Item.AttributeStat.CritRate].AddModifier(AddCritRate);
    }

    float AddCritRate(float input)
    {
        return input + 15;
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        if (targets[0].IsDead)
        {
            //give attack buff
        }
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.CritRate].RemoveModifier(id);
    }
}