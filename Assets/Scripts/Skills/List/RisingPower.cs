using System.Collections.Generic;

public class RisingPower : PassiveSkill
{
    private float _attackBaseRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        float attackBuffRatio = _attackBaseRatio + (StatUpgrade1 * Level);
        float attackBuff;
        /*if(weapon != two-handed sword)*/
        if (caster.Weapon.Type != 0)
        {
            attackBuff = caster.Stats[Attribute.Attack].Value * attackBuffRatio;
        }else
        {
            attackBuff = caster.Stats[Attribute.Attack].Value * attackBuffRatio * 2;     
        }
        //player.Attack += attackBuff; Modifier
    }
}