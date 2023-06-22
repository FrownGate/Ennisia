using System.Collections.Generic;
using static Stat<float>;



public class Alchemy : PassiveSkill
{
    ModifierID id;
    public float magicAtkBaseRatio;
    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
      id = player.Stats[Item.AttributeStat.MagicalDamages].AddModifier(MRatioBuff);
    }

    float MRatioBuff(float input)
    {
        return input * (magicAtkBaseRatio + StatUpgrade1 * Level);
    }
    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        if (turn %2 == 0)
        {
            player.Skills[2].Cooldown -= 1;
        }
    }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        player.Stats[Item.AttributeStat.MagicalDamages].RemoveModifier(id);
    }


}
