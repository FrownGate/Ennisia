using System.Collections.Generic;

public class Alchemy : Skill
{
    public float magicAtkBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float MRatioBuff = player.MagicAtk * (magicAtkBaseRatio + StatUpgrade1 * Level);
        player.MagicAtk += MRatioBuff;
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        if (turn %2 == 0)
        {
            player.Skills[2].Cooldown -= 1;
        }
    }
}
