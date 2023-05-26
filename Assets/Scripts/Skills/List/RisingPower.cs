using System.Collections.Generic;

public class RisingPower : Skill
{
    private void Awake()
    {
        FileName = "RisingPower";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        /*if(weapon != two-handed sword)*/
        float AttackBuff = player.Attack * 0.15f;
        /*if(weapon == two-handed sword)*/
        //float AttackBuff = player.Damage * 0.30f;
        player.Attack += AttackBuff;
    }
}