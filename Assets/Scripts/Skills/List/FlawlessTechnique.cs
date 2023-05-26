using System.Collections.Generic;

public class FlawlessTechnique : Skill
{
    private void Awake()
    {
        FileName = "FlawlessTechnique";
    }

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        //critRate + 15%
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        if (targets[0].IsDead)
        {
            //give attack buff
        }
    }
}