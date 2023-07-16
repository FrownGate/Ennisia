using System.Collections.Generic;

public class LastHope : PassiveSkill
{
    private float _healBaseRatio;
    private bool _isUsed = false;

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        float healBuff = _healBaseRatio + StatUpgrade1 * Level;

        if (caster.CurrentHp < caster.Stats[Attribute.HP].Value * 0.2f & !_isUsed)
        {
            _isUsed = true;
            HealingModifier = caster.Stats[Attribute.HP].Value * healBuff;
            caster.Heal(HealingModifier);
        }
    }
}