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
    DemonicMark = 4,
}

public class BuffEffect
{
    public string Name  { get; set; }
    public string Description { get; set; }
    public ModifierID Id { get; set; }
    public int Duration { get; set; }
    public int InitialDuration { get; set; }
    public float ModifierValue { get; set; }
    private bool IsExpired => Duration <= 0;
    
    public List<Item.AttributeStat> ModifiedStats { get; set; } = new();
    public AlterationState State { get; set; } = AlterationState.None;

    public BuffEffect()
    {
        Duration = 0;
        InitialDuration = 0;
        ModifierValue = 0;
        State = AlterationState.None;
    }
    
    //Alteration state constructor
    public BuffEffect(int duration, AlterationState state, string name ="", string description="")
    {
        Duration = duration;
        InitialDuration = duration;
        State = state;
    }
    //Buff or debuff constructor
    public BuffEffect(int duration, Item.AttributeStat statToModify, float modifierValue, string name ="", string description="")
    {
        Duration = duration;
        InitialDuration = duration;
        ModifiedStats = new List<Item.AttributeStat>{statToModify};
        ModifierValue = modifierValue;
    }
    //Buff and debuff affect multiple stats constructor
    public BuffEffect(int duration, List<Item.AttributeStat> statsToModify, float modifierValue, string name ="", string description="")
    {
        Duration = duration;
        InitialDuration = duration;
        ModifierValue = modifierValue;
        ModifiedStats = new(statsToModify);
    }
    
    //Alteration and buff/debuff constructor
    public BuffEffect(int duration, List<Item.AttributeStat> statsToModify, float modifierValue, AlterationState state, string name ="", string description="")
    {
        Duration = duration;
        InitialDuration = duration;
        ModifiedStats = new(statsToModify);
        ModifierValue = modifierValue;
        State = state;
        //Id = target.AlterateStat(statToModify, value => value * modifierValue, 1);
    }
    
    public void Tick(Entity _target)
    {
        Duration--;
        if (IsExpired)
        {
            foreach (var modifiedStat in ModifiedStats)
            {
                _target.Stats[modifiedStat].RemoveModifier(Id);
            }
            _target.Buffs.Remove(this);
        }
    }

    public void ResetDuration()
    {
        Duration = InitialDuration;
    }
    
}