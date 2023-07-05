using System.Collections.Generic;

public class KnightOath : PassiveSkill
{
    private float _BuffBaseRatio;
    float buffMaxHp;
    float buffPhDef;
    float buffMDef;
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _BuffBaseRatio = 0.1f + StatUpgrade1 * Level;
        float buffMaxHp = caster.Stats[Attribute.HP].Value * _BuffBaseRatio;
        float buffPhDef = caster.Stats[Attribute.PhysicalDefense].Value * _BuffBaseRatio;
        float buffMDef = caster.Stats[Attribute.MagicalDefense].Value * _BuffBaseRatio;
        _modifiers[Attribute.HP] = caster.Stats[Attribute.PhysicalDefense].AddModifier(HPBuff);
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(PHDefBuff);
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(MDefBuff);

    }


    float HPBuff(float value) => value + buffMaxHp;

    float PHDefBuff(float value) => value + buffPhDef;

    float MDefBuff(float value) => value + buffMDef;


    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken) //caster + player ?
    {
        player.Shield += player.Stats[Attribute.HP].Value * 0.05f;
    }
}