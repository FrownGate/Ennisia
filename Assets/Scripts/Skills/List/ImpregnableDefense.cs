using System.Collections.Generic;
using static Stat<float>;

public class ImpregnableDefense : PassiveSkill
{

    private List<ModifierID> _defID = new List<ModifierID>();
    private List<ModifierID> _magicalDefID = new List<ModifierID>();

    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {

        if(turn < 5)
        {
            _defID.Add(player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(Add5PercentOfDef));
            _magicalDefID.Add(player.Stats[Item.AttributeStat.MagicalDefense].AddModifier(Add5PercentOfDef));
        }
    }

    float Add5PercentOfDef(float input)
    {
        return input + (5/100 * input);
    }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn)
    {
        foreach(var id in _defID)
        {
            player.Stats[Item.AttributeStat.PhysicalDefense].RemoveModifier(id);
        }
        foreach (var id in _magicalDefID)
        {
            player.Stats[Item.AttributeStat.MagicalDefense].RemoveModifier(id);
        }
    }
}