public class Silence : Effect
{
    public Silence(int? duration = null) : base(duration) { }

    public override void AlterationEffect(Entity target, Entity caster = null)
    {
        for (int i = 1; i < target.Skills.Count; i++)
        {
            // FIXME: ToggleUse doesn't work
            target.Skills[i].Button.ToggleUse(false);
        }

        //TODO -> Check if buttons are toggled back if no silence
    }
}