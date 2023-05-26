using System.Collections.Generic;

public class Blessing : Skill
{
    private void Awake()
    {
        FileName = "Blessing";
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        HealingModifier = damage * 0.1f;

        if (turn % 2 == 0)
        {
            // to do : give immunity
        }
    }
}