using System.Collections.Generic;

public class Blessing : PassiveSkill
{

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        caster.Heal((Data.HealingAmount / 100) + (StatUpgrade1 * Level));


        if (turn % 2 == 0)
        {
            caster.ApplyEffect(new AttackBuff());
        }
    }
}