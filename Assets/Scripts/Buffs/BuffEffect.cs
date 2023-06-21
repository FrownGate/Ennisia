using System;
using System.Collections.Generic;
using PlayFab.ProfilesModels;
using UnityEngine.UI;
public enum AlterationState
{
    None = 0,
    Stun = 1,
    Silence = 2,
    SupportSilence = 3,
}

public class BuffEffect
{
    public string Description { get; set; }
    public string Name { get; set; }
    public ModifierID Id { get; set; }
    private bool IsExpired => Duration <= 0;
    private int Duration { get; set; }
    
    private readonly Item.AttributeStat _modifiedStat;
    public  AlterationState State { get; private set; } = AlterationState.None;
    
    //Alteration state constructor
    public BuffEffect(int duration, AlterationState state)
    {
        Duration = duration;
        State = state;
    }
    
    
    //Buff or debuff constructor
    public BuffEffect(int duration, Entity target, Item.AttributeStat statToModify, float modifierValue)
    {
        Duration = duration;
        _modifiedStat = statToModify;
        Id = target.AlterateStat(statToModify, value => value * modifierValue, 1);
    }
    
    public void Tick(Entity _target)
    {
        Duration--;
        if (IsExpired)
        {
            _target.Stats[_modifiedStat].RemoveModifier(Id);
            _target.Buffs.Remove(this);
        }
    }

    public void ResetDuration(int duration)
    {
        Duration = duration;
    }
    
}