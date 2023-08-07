public class Silence : Effect
{
    public Silence(int? duration = null) : base(duration) { }

    public override void AlterationEffect()
    {
        for (int i = 1; i < Target.Skills.Count; i++)
        {
            Target.Skills[i].Button.ToggleUse(false);
        }

        //TODO -> Check if buttons are toggled back if no silence
    }
}