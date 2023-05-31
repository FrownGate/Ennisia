using System.Collections.Generic;

public class Alchemy : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float MRatioBuff = player.MagicAtk * 0.5f;
        player.MagicAtk += MRatioBuff;
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        if (turn %2 == 0)
        {
            player.WeaponSO._skill2.Cooldown -= 1;
        }
    }
}
