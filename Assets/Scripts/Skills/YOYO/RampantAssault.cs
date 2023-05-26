public class RampantAssault : Skill
{
    private const float _percentagePerTurn = 0.05f;
    private readonly float _damagePercentage = 0.2f;

    private void Awake()
    {
        fileName = "RampantAssault";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        damageModifier = targets[0].maxHp * _damagePercentage;
        float percOfAddDamage = _percentagePerTurn * turn;

        percOfAddDamage = percOfAddDamage > 0.5f ? 0.5f : percOfAddDamage;

        if (percOfAddDamage > 0.5f)
        {
            percOfAddDamage = 0.5f;
        }

        float damagePerTurn = damageModifier * percOfAddDamage;
        damageModifier += damagePerTurn;
        return damageModifier;
    }
}
