using System.Collections.Generic;

public abstract class Effect
{
    public string Description { get; set; }
    public string Name { get; set; }
    protected internal List<string> ModifiedStats { get; protected set; } //Use enum instead of strings
    protected internal int Duration { get; protected set; }
    //Define percentages for break or buff here

    public void Tick()
    {
        Duration--;
    }

    public void ResetDuration(int duration)
    {
        Duration = duration;
    }

}