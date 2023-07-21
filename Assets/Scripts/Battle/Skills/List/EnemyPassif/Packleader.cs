using System.Collections.Generic;

public class PackLeader : PassiveSkill
{
    private bool _hasModifier = false;
    private float _additionalStatIncrease;

    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Entity target in targets)
        {
            _additionalStatIncrease += 0.05f;

        }
        if (targets.Count >= 2 && !_hasModifier)
        {
            foreach (var stat in caster.Stats)
            {
                if (stat.Key == Attribute.DefIgnored) continue;
                _modifiers[stat.Key] = caster.Stats[stat.Key].AddModifier(AllStatBuf);
            }

            _hasModifier = true;
        }
        else if (targets.Count <= 2 && _hasModifier)
        {
            _hasModifier = false;
            TakeOffStats(caster);
        }
    }

    float AllStatBuf(float value) => value * (1 + (Data.BuffAmount/100) + _additionalStatIncrease);
}