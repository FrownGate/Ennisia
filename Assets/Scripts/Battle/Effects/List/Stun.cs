public class Stun : Effect
{
    public Stun(int? duration = null) : base(duration) { }

    public override void AlterationEffect()
    {
        //if (Target.Effects.Find(effect => effect.GetType() == typeof(Stun)) == null)
        //{
        //    if (Target.AtkBarPercentage == 100)
        //    {
        //        Target.ResetAtb();
        //    }
        //}

        if (Target.AtkBarPercentage == 100)
        {
            Target.ResetAtb();
        }
    }
}