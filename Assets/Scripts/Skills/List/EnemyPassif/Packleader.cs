using System.Collections.Generic;

public class PackLeader : PassiveSkill
{
    private bool _hasModifier = false;
    private float _additionalStatIncrease;

    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        for (int i = 2; i < targets.Count; i++)
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

    float AllStatBuf(float value) => (float)value * (1.10f + _additionalStatIncrease);
}