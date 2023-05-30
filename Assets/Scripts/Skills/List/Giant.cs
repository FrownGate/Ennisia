using System.Collections.Generic;

public class Giant : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float maxHpBuff = player.MaxHp * 0.05f;
        player.Attack = maxHpBuff;
    }
}