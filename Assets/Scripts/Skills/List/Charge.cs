using System.Collections.Generic;

public class Charge : Skill
{
    public float attackBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float attackRatioBuff = attackBaseRatio + StatUpgrade1 * Level;
        float attackBuff = player.Attack * attackRatioBuff;
        player.Attack = attackBuff;
    }
}