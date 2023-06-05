using System.Collections.Generic;

public class LastHope : Skill
{
    public float healBaseRatio;
    private bool _isUsed = false;

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        float healBuff = healBaseRatio + StatUpgrade1 * Level;
        if (player.CurrentHp < player.MaxHp * 0.2f & !_isUsed)
        {
            _isUsed = true;
            HealingModifier = player.MaxHp * healBuff;
        }
    }
}