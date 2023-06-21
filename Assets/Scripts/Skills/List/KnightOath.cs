using JetBrains.Annotations;
using System.Collections.Generic;

public class KnightOath : Skill
{
    public float BuffBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        BuffBaseRatio = 0.1f + StatUpgrade1 * Level;
        float buffMaxHp = player.Stats[Item.AttributeStat.HP].Value * BuffBaseRatio;
        float buffPhDef = player.Stats[Item.AttributeStat.PhysicalDefense].Value * BuffBaseRatio;
        float buffMDef = player.Stats[Item.AttributeStat.MagicalDefense].Value * BuffBaseRatio;
      /*  player.MaxHp += buffMaxHp;
        player.PhysDef += buffPhDef;
        player.MagicDef += buffMDef;*/ //modifiers
    }

    //add revenge : after receiving dmg, give a shield of 5% of max hp :: shieldModifier = player.MaxHp * 0.05f; ;
}