using System.Collections.Generic;

public class LastHope : PassiveSkill
{
    private bool _isUsed = false;

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        float healBuff = (Data.HealingAmount/100) + StatUpgrade1 * Level;

        if (caster.CurrentHp < caster.Stats[Attribute.HP].Value * 0.2f & !_isUsed)
        {
            _isUsed = true;
            caster.Heal(caster.Stats[Attribute.HP].Value * healBuff);
        }
    }
}