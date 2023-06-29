using System.Collections.Generic;

public class BigMushroom : PassiveSkill
{
    ModifierID id;
    bool hasModifier = false;
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {
        if (targets.Count <= 2 && !hasModifier)
        {
            id = player.Stats[Item.AttributeStat.Attack].AddModifier(AttackBuf);
            hasModifier = true;
        }
        else if (targets.Count >= 2)
        {
            hasModifier = false;
        }
    }
    float AttackBuf(float input)
    {
        return (float)input * 2;
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {
        float heal = damage;

        // add heal function
    }
}