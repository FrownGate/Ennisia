using System.Collections.Generic;

public class Blessing : Skill
{
    public float healBaseRatio;
    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        float healBuffRatio = healBaseRatio + StatUpgrade1 * Level;
        HealingModifier = damage * healBuffRatio;

        if (turn % 2 == 0)
        {
            // to do : give immunity
        }
    }
}