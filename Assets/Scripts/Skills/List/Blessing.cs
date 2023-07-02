using System.Collections.Generic;

public class Blessing : PassiveSkill
{
    private float _healBaseRatio;

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        float healBuffRatio = _healBaseRatio + StatUpgrade1 * Level;
        HealingModifier = damage * healBuffRatio;

        if (turn % 2 == 0)
        {
            // to do : give immunity
        }
    }
}