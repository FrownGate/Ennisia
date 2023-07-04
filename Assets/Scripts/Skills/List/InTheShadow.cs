using System.Collections.Generic;

public class InTheShadow : DamageSkill
{
    private float _defIgnoredBaseRatio;
    private float _defIgnoredBuff;

    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        _defIgnoredBuff = _defIgnoredBaseRatio + (StatUpgrade1 * Level);
        _modifiers[Attribute.DefIgnored] = targets[0].Stats[Attribute.DefIgnored].AddModifier(IgnoreDef); //targets[0] or caster ?
    }

    float IgnoreDef(float value) => value + 40;

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        //if debuffed
        player.AtkBarPercentage = 100;
    }
    // to do : if enemy is debuff, #% chance to play again
}