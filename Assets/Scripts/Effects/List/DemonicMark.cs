public class DemonicMark : Effect
{
    public override bool IsStackable => true;
    private const int _maxStacks = 10;
    private const float _damageIncreasePercentage = 0.05f;

    public DemonicMark(int? duration = null) : base(duration) { }

    public override void ApplyStack(int stack)
    {
        if (Stacks + stack > _maxStacks)
        {
            Stacks = _maxStacks;
        }
        else if (Stacks + stack <= _maxStacks)
        {
            Stacks += stack;
        }
    }

    public override float GetMultiplier() { return 1.0f + (_damageIncreasePercentage * Stacks); }
}