using System.Collections.Generic;
using static Stat<float>;

public class Swiftness : Skill
{
    public float speedRatio;
    float speedRatioBuff;
    ModifierID id;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        speedRatioBuff = speedRatio + StatUpgrade1 * Level;
        id = player.Stats[Item.AttributeStat.Speed].AddModifier(Speed);
    }

    float Speed(float input) { return speedRatioBuff + input; }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Speed].RemoveModifier(id);
    }
}