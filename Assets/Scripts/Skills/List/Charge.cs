using System.Collections.Generic;

public class Charge : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float attackBuff = player.Attack * 0.05f;
        player.Attack = attackBuff;
    }
}