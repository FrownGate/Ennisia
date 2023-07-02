using System.Collections.Generic;

public class Swiftness : PassiveSkill
{
    private float _speedRatio;
    private float _speedRatioBuff;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _speedRatioBuff = _speedRatio + StatUpgrade1 * Level;
        _modifiers[Attribute.Speed] = caster.Stats[Attribute.Speed].AddModifier(Speed);
    }

    float Speed(float value) => _speedRatioBuff + value;
}