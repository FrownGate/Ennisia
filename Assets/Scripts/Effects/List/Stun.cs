public class Stun : Effect
{
    public override void AlterationEffect(Entity target)
    {
        target.ResetAtb();
    }
}