using System.Collections.Generic;

public class DruidInstinct : Skill
{
    public float maxHpBaseRatio;
    public float healOnDmg;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float maxHpBuff = player.Attack * (maxHpBaseRatio + StatUpgrade1 * Level);
        player.MaxHp += maxHpBuff;
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        healOnDmg = 0.03f + StatUpgrade2 * Level;
        HealingModifier = damage * healOnDmg;
    }

    //add revenge : after receiving dmg, give a shield of 5% of max hp 
}