using System.Collections.Generic;

public class FairyTales : Skill
{
    private float _magicBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDamages] = caster.Stats[Attribute.MagicalDamages].AddModifier(MagicBuff);
    }

    float MagicBuff(float input) => input * _magicBuffRatio; /*TO DO scale on speed : magicBuff * player.speed )*/
}