public class Taunt : Effect
{
    public Taunt(int? duration = null, Entity? caster = null) : base(duration, caster) { }

    public override void AlterationEffect(Entity entity, Entity caster)
    {
    }
}