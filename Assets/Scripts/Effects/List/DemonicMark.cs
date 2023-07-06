using UnityEngine;
public class DemonicMark : Effect
{
    public override bool IsStackable => true;
    public override int Stacks { get; protected set; } = 0;
    private const int MaxStacks = 10;
    private const float _damageIncreasePercentage = 0.05f;
    public DemonicMark(int? duration = null) : base(duration) { }

    public override void ApplyStack(int stack)
    {
        if (Stacks + stack > MaxStacks)
        {
            Stacks = MaxStacks;
        }
        else if (Stacks + stack <= MaxStacks)
        {
            Stacks += stack;
        }
    }

    public override float GetMultiplier() { return 1.0f + (_damageIncreasePercentage * Stacks); }
}