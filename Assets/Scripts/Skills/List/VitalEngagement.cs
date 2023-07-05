using System.Collections.Generic;

public class VitalEngagement : Skill
{
    public float AttackBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        _modifiers[Attribute.Attack] = player.Stats[Attribute.Attack].AddModifier(AttackBuff);
        AttackBuffRatio = player.Stats[Attribute.HP].Value;
    }

    //TODO -> scale on HP : AttackBuffRatio * player.HP
    float AttackBuff(float value) => value * AttackBuffRatio;
}