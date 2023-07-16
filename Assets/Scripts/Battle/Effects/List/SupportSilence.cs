public class SupportSilence : Effect
{
    public SupportSilence(int? duration = null) : base(duration) { }

    public override void AlterationEffect(Entity target)
    {
        foreach (var support in target.EquippedSupports)
        {
            foreach (var skill in support.Skills)
            {
                skill.Button.ToggleUse(false);
            }
        }
    }
}