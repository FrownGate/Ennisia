using System.Collections.Generic;
using static Stat<float>;
public class MakeOrBreak : PassiveSkill
{
    ModifierID id;
    public float maxHpBaseRatio;
    public float healOnDmg;
    float maxHpBuff;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        maxHpBuff = player.Stats[Item.AttributeStat.Attack].Value * (maxHpBaseRatio + StatUpgrade1 * Level);
        player.Stats[Item.AttributeStat.Attack].AddModifier(AttackBuff);
    }

    float AttackBuff(float input)
    {
        return (float)input + maxHpBuff;
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        healOnDmg = 0.03f + StatUpgrade2 * Level;
        HealingModifier = damage * healOnDmg;
    }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Attack].RemoveModifier(id);
    }
}
