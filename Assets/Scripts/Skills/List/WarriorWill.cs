using System.Collections.Generic;

public class WarriorWill : Skill
{
    private float _physicalDmgBuffRatio;
    private float _physicalDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        _modifiers[Attribute.PhysicalDamages] = player.Stats[Attribute.PhysicalDamages].AddModifier(PhysicalBuff);
        _modifiers[Attribute.PhysicalDefense] = player.Stats[Attribute.PhysicalDefense].AddModifier(PhysicalDefBuff);
    }

    float PhysicalBuff(float value) => value * (1 + _physicalDmgBuffRatio);
    float PhysicalDefBuff(float value) => value * (1 + _physicalDefBuffRatio);
}