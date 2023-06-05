using System.Collections.Generic;

public class RisingPower : Skill
{
    public float AttackBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float attackBuffRatio = AttackBaseRatio + (StatUpgrade1 * Level);
        float attackBuff;
        /*if(weapon != two-handed sword)*/
        if (player.WeaponSO.Type != 0)
        {
            attackBuff = player.Attack * attackBuffRatio;
        }else
        {
            attackBuff = player.Attack * attackBuffRatio * 2;     
        }
        player.Attack += attackBuff;
    }
    
}