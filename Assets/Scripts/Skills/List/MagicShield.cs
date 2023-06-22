using System.Collections.Generic;

public class MagicShield : Skill
{
    ModifierID id;
    public float MagicDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.MagicalDefense].AddModifier(MagicShieldBuff);
    }
    float MagicShieldBuff(float input)
    {
        return input * (1 + MagicDefBuffRatio);
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.MagicalDefense].RemoveModifier(id);
    }
}
