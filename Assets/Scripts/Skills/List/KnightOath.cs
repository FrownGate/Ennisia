using JetBrains.Annotations;
using System.Collections.Generic;

public class KnightOath : Skill
{
    public float BuffBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        BuffBaseRatio = 0.1f + StatUpgrade1 * Level;
        float buffMaxHp = player.MaxHp * BuffBaseRatio;
        float buffPhDef = player.PhysDef * BuffBaseRatio;
        float buffMDef = player.MagicDef * BuffBaseRatio;
        player.MaxHp += buffMaxHp;
        player.PhysDef += buffPhDef;
        player.MagicDef += buffMDef;
    }

    //add revenge : after receiving dmg, give a shield of 5% of max hp :: shieldModifier = player.MaxHp * 0.05f; ;
}