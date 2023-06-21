using System;
using System.Collections.Generic;
using PlayFab.ProfilesModels;
using UnityEngine.UI;



public class BuffEffect
{
    public string Description { get; set; }
    public string Name { get; set; }
    protected  List<Item.AttributeStat> ModifiedStats { get;  set; } //Use enum instead of strings
    private readonly Action<Entity> _targetAction;
    protected  int Duration { get;  set; }
    //Define percentages for break or buff here
    
    public BuffEffect(int duration, Entity taget ,Action<Entity>targetAction)
    {
        ModifiedStats = new();
        Duration = duration;
        _targetAction = targetAction;
        _targetAction?.Invoke(taget);
    }
    
    public BuffEffect(int duration, Action<Entity, Entity >target )
    {
        ModifiedStats = new();
        Duration = duration;

    }
    
    public void UseOn(Entity target)
    {
    }
    

    public void Tick()
    {
        Duration--;
    }

    public void ResetDuration(int duration)
    {
        Duration = duration;
    }
    
}