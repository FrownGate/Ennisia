using System.Collections.Generic;


public class FairyTales : Skill
{
    ModifierID id;
    public float MagicBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {

        id = player.Stats[Item.AttributeStat.MagicalDamages].AddModifier(MagicBuff);
    }
    float MagicBuff(float input)
    {
        return input * MagicBuffRatio /*TO DO scale on speed : magicBuff * player.speed )*/ ;
    }
    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.MagicalDamages].RemoveModifier(id);
    }
}
