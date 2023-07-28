using System.Collections.Generic;

public class KnightOath : PassiveSkill
{
    private float _BuffBaseRatio;
    float buffMaxHp;
    float buffPhDef;
    float buffMDef;
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn, List<Entity> allies)
    {
        _BuffBaseRatio = (Data.BuffAmount/100) + (StatUpgrade1 * Level);
        buffMaxHp = caster.Stats[Attribute.HP].Value * _BuffBaseRatio;
        buffPhDef = caster.Stats[Attribute.PhysicalDefense].Value * _BuffBaseRatio;
        buffMDef = caster.Stats[Attribute.MagicalDefense].Value * _BuffBaseRatio;
        _modifiers[Attribute.HP] = caster.Stats[Attribute.PhysicalDefense].AddModifier(HPBuff);
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(PHDefBuff);
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(MDefBuff);

    }


    float HPBuff(float value) => value + buffMaxHp;

    float PHDefBuff(float value) => value + buffPhDef;

    float MDefBuff(float value) => value + buffMDef;


    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken,
        List<Entity> allies) //caster + player ?
    {
        player.Shield += player.Stats[Attribute.HP].Value * (Data.ShieldAmount / 100);
    }
}   