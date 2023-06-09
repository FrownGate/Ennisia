using System.Collections.Generic;

public class ATKBUFF : BuffEffect
{
    private float _percentage => 1.5f;

    public ATKBUFF(int duration, Entity target)
    {
        ModifiedStats = new List<string> { "Attack" };
        Duration = duration;
        target.Attack *= _percentage;
    }
}