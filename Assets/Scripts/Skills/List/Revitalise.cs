using System.Collections.Generic;


public class Revitalise : Skill
{
    ModifierID id;
    public float HpBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.HP].AddModifier(HpBuff);
    }
    float HpBuff(float input)
    {
        return input * (1 + HpBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.HP].RemoveModifier(id);
    }
}
