using System.Collections.Generic;

public class RisingPower : Skill
{
    public float AttackBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float attackBuffRatio = AttackBaseRatio + (StatUpgrade1 * Level);
        float attackBuff;
        /*if(weapon != two-handed sword)*/
        if (player.Weapon.Type != 0)
        {
            attackBuff = player.Stats[Item.AttributeStat.Attack].Value * attackBuffRatio;
        }else
        {
            attackBuff = player.Stats[Item.AttributeStat.Attack].Value * attackBuffRatio * 2;     
        }
        //player.Attack += attackBuff; Modifier
    }
    
}