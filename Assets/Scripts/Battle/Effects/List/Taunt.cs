public class Taunt : Effect
{
    public Taunt(int? duration = null) : base(duration) { }

    public override void AlterationEffect(Entity target, Entity caster = null)
    {
        // target.Skills[0].Use();
    }
}