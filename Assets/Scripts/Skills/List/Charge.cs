using System.Collections.Generic;
using static Stat<float>;

public class Charge : DamageSkill
{ 
    public float attackBaseRatio;
    float attackRatioBuff;
    ModifierID id;
    
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        attackRatioBuff = attackBaseRatio + StatUpgrade1 * Level;
        id = player.Stats[Item.AttributeStat.Attack].AddModifier(AddAttackBuff);
    }

    float AddAttackBuff(float input) { return attackBaseRatio + input; }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Attack].RemoveModifier(id);
    }
}