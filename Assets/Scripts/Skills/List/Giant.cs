using System.Collections.Generic;
using static Stat<float>;

public class Giant : PassiveSkill
{
    ModifierID id;
    public float healthBaseRatio;
    public float healthRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        healthRatio = healthBaseRatio + StatUpgrade1 * Level;
        id = player.Stats[Item.AttributeStat.HP].AddModifier(MaxHPBuff);
    }
    float MaxHPBuff(float input)
    {
        return input * healthRatio;
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.HP].RemoveModifier(id);
    }
}