using System.Collections.Generic;

public class PackLeader : PassiveSkill
{
    List<ModifierID> id;
    bool hasModifier = false;
    float amountOfAllies;
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {
        for (int i = 2; i < targets.Count; i++)
        {
            amountOfAllies += 0.05f;
        }
        if (targets.Count >= 2 && !hasModifier)
        {
            id[0] = player.Stats[Item.AttributeStat.Attack].AddModifier(AllStatBuf);
            id[1] = player.Stats[Item.AttributeStat.CritDmg].AddModifier(AllStatBuf);
            id[2] = player.Stats[Item.AttributeStat.CritRate].AddModifier(AllStatBuf);
            id[3] = player.Stats[Item.AttributeStat.HP].AddModifier(AllStatBuf);
            id[4] = player.Stats[Item.AttributeStat.MagicalDefense].AddModifier(AllStatBuf);
            id[5] = player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(AllStatBuf);
            hasModifier = true;
        }
        else if (targets.Count <= 2)
        {
            hasModifier = false;
        }
    }
    float AllStatBuf(float input)
    {
        return (float)input * (1.10f + amountOfAllies);
    }
}