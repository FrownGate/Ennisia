using System.Collections.Generic;

public class Giant : Skill
{
    public float healthBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float healthRatio = healthBaseRatio + StatUpgrade1 * Level;
        float maxHpBuff = player.MaxHp * healthRatio;
        player.MaxHp = maxHpBuff;
    }
}