using System.Collections.Generic;

public class EffectApplier
{
    public static EffectApplier Instance { get; } = new EffectApplier();
    public Dictionary<string, Effect> EffectDatabase { get; set; }
    public Effect BuffToApply { get; set; }
    public Effect AlterationToApply { get; set; }
    public EffectApplier()
    {
        //InitializeEffectDatabase();
    }

    //public void ApplyEffectTo(Entity target, Effect buffEffectToApply)
    //{
    //    BuffToApply = new Effect();
    //    BuffToApply = buffEffectToApply;
    //    var existingBuff = target.Buffs.FirstOrDefault(st => st.Name == buffEffectToApply.Name);
    //    if (existingBuff != null)
    //    {
    //        existingBuff.ResetDuration();
    //        return;
    //    }
    //    foreach (var modifiedStat in BuffToApply.ModifiedStats)
    //    {
    //        BuffToApply.Modifiers = target.AlterateStat(modifiedStat, value => value * BuffToApply.ModifierValue, 1);
    //    }
    //    target.AddBuffEffect(BuffToApply);
    //}

    //public void ApplyAlterationTo(Entity target, Effect alterationToApply)
    //{
    //    AlterationToApply = new Effect();
    //    AlterationToApply = alterationToApply;
    //    var existingAlt = target.Alterations.FirstOrDefault(a => a.State.ToString() == alterationToApply.State.ToString());
    //    if (existingAlt != null)
    //    {
    //        existingAlt.ResetDuration();
    //        return;
    //    }
    //    target.AddAlteration(AlterationToApply);
    //}

    //private void InitializeEffectDatabase()
    //{
    //    EffectDatabase = new Dictionary<string, Effect>()
    //    {
    //        //Buffs
    //        {EffectType.ATKBUFF.ToString(),new Effect(3,Item.AttributeStat.Attack,1.5f,"ATKBUFF","Increase attack")},
    //        {EffectType.DEFENSEBUFF.ToString(), new Effect(3,new List<Item.AttributeStat>
    //            {   Item.AttributeStat.PhysicalDefense, 
    //                Item.AttributeStat.MagicalDefense
    //            },
    //            1.5f,"DEFENSEBUFF","Increase defense")
    //        },
    //        {Item.AttributeStat.CritRate.ToString(),new Effect(3, Item.AttributeStat.CritRate,1.5f,"CritRate","Increase crit rate")},
    //        {Item.AttributeStat.CritDmg.ToString(), new Effect(3,Item.AttributeStat.CritDmg,1.5f,"CritDmg","Increase crit damage")},

    //        //Alterations
    //        {AlterationState.Silence.ToString(), new Effect(3,AlterationState.Silence, "Silence")},
    //        {AlterationState.Stun.ToString(), new Effect(3,AlterationState.Stun, "Stun")},
    //        {AlterationState.DemonicMark.ToString(), new Effect(3, AlterationState.DemonicMark, "DemonicMark")},
    //        {AlterationState.None.ToString(), new Effect()},

    //        //Debuff
    //        {EffectType.BREAKDEFENSE.ToString(), new Effect(3,new List<Item.AttributeStat>
    //            {
    //            Item.AttributeStat.MagicalDefense,
    //            Item.AttributeStat.PhysicalDefense
    //            },
    //            0.7f,"BREAKDEFENSE","Decrease defense")
    //        },
    //        {EffectType.BREAKATTACK.ToString(),new Effect(3,Item.AttributeStat.Attack,0.7f,"BREAKATTACK","Decrease attack")}
    //    };
    //}

}