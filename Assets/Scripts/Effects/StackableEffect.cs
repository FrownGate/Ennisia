abstract public class StackableEffect : Effect
{
    public int Stacks { get; set; } = 0;
    public int MaxStacks { get; set; } = 1;

    protected StackableEffect(int? duration = null) : base(duration) { }

    protected virtual float GetMultiplier() { return 1.0f; }
    
}