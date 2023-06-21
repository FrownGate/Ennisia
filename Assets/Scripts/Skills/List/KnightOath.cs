using JetBrains.Annotations;
using System.Collections.Generic;

public class KnightOath : PassiveSkill
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


    public override void UseIfAttacked(List<Entity> targets, Entity player, int turn, float damageTaken)
    {
        player.Shield += player.Stats[Item.AttributeStat.HP].Value * 0.05f;
    }
   
}