using System.Collections.Generic;

public class KnightOath : PassiveSkill
{
    private float _BuffBaseRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _BuffBaseRatio = 0.1f + StatUpgrade1 * Level;
        float buffMaxHp = caster.Stats[Attribute.HP].Value * _BuffBaseRatio;
        float buffPhDef = caster.Stats[Attribute.PhysicalDefense].Value * _BuffBaseRatio;
        float buffMDef = caster.Stats[Attribute.MagicalDefense].Value * _BuffBaseRatio;
      /*  player.MaxHp += buffMaxHp;
        player.PhysDef += buffPhDef;
        player.MagicDef += buffMDef;*/ //modifiers
    }

    public override void UseIfAttacked(List<Entity> targets,Entity caster, Entity player, int turn, float damageTaken) //caster + player ?
    {
        player.Shield += player.Stats[Attribute.HP].Value * 0.05f;
    }
}