using System.Collections.Generic;

public class RisingPower : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float AttackBuff = 0;
        /*if(weapon != two-handed sword)*/
        if (player.WeaponSO.weaponType != 0)
        {
            AttackBuff = player.Attack * 0.15f;
        }else
        {
            AttackBuff = player.Attack * 0.30f;
        }
        player.Attack += AttackBuff;
    }
}