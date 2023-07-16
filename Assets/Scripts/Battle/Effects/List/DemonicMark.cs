public class DemonicMark : Effect
{
    private const float _damageIncreasePercentage = 0.05f;

    public DemonicMark(int? duration = null) : base(duration)
    {
        IsStackable = true;
        _maxStacks = 10;
    }

    public override float GetMultiplier() { return 1.0f + (_damageIncreasePercentage * Stacks); }
}