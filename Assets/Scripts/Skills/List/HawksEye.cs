using System.Collections.Generic;

public class HawksEye : BuffSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //give CR / CD / ATTACK Buff
        //give additional turn
        return 0;
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        player.AtkBarPercentage = 100;
    }
}