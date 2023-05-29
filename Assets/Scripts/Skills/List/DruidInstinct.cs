using System.Collections.Generic;

public class DruidInstinct : Skill
{
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float MaxHpBuff = player.Attack * 1.5f;
        player.MaxHp += MaxHpBuff;
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        HealingModifier = damage * 0.05f;
    }

    //add revenge : after receiving dmg, give a shield of 5% of max hp 
}