using System.Collections.Generic;

public class BREAKATK : BuffEffect
{
    private float _percentage => 0.7f;

    public BREAKATK(int duration, Entity target)
    {
        ModifiedStats = new List<string> { "Attack" };
        Duration = duration;
        target.Attack *= _percentage;
    }
}