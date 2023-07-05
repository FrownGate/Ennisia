public class DemonicMark : StackableEffect
{
    private const float _damageIncreasePercentage = 0.05f;

    public DemonicMark(int stacks, int? duration = null) : base(duration)
    {
        MaxStacks = 10;

        if (true)
        {
            Stacks += stacks;
        }
    }
    protected override float GetMultiplier() { return 1.0f + (_damageIncreasePercentage * Stacks); }
}