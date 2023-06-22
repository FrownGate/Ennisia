using System.Collections.Generic;


public class VitalEngagement : Skill
{
    ModifierID id;
    public float AttackBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.Attack].AddModifier(AttackBuff);
    }
    float AttackBuff(float input)
    {
        return input * AttackBuffRatio /*TO DO scale on HP : AttackBuffRatio * player.HP )*/ ;
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.Attack].RemoveModifier(id);
    }
}
