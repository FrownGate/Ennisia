public class Stun : Effect
{
    public Stun(int? duration = null) : base(duration) { }

    public override void AlterationEffect(Entity target, Entity caster = null)
    {
        if (target.Effects.Find(effect => effect.GetType() == typeof(Stun)) == null)
        {
            if (target.AtkBarPercentage == 100)
            {
                target.ResetAtb();
            }
        }
    }
}