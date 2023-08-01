public class SupportSilence : Effect
{
    public SupportSilence(int? duration = null) : base(duration) { }

    public override void AlterationEffect()
    {
        foreach (var support in Target.EquippedSupports)
        {
            foreach (var skill in support.Skills)
            {
                skill.Button.ToggleUse(false);
            }
        }
    }
}