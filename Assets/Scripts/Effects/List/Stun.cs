public class Stun : Effect
{
    public Stun(int? duration = null) : base(duration) { }

    public override void AlterationEffect(Entity target)
    {
        target.ResetAtb();
    }
}