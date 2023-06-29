
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;

    public enum EffectType
    {
        ATKBUFF = 0,
        DEFENSEBUFF = 1,
        BREAKDEFENSE = 2,
        BREAKATTACK = 3,
    }
    
    
    public class EffectApplier
    {
        public static EffectApplier Instance { get; } = new EffectApplier();
        public Dictionary<string,BuffEffect> EffectDatabase { get; set; } 
        public BuffEffect BuffToApply { get; set; }
        public BuffEffect AlterationToApply { get; set; }
        public EffectApplier()
        {
            InitializeEffectDatabase();
        }

        public void ApplyEffectTo(Entity target, BuffEffect buffEffectToApply)
        {
            BuffToApply = new BuffEffect();
            BuffToApply = buffEffectToApply;
            var existingBuff = target.Buffs.FirstOrDefault(st => st.Name == buffEffectToApply.Name);
            if (existingBuff != null)
            {
                existingBuff.ResetDuration();
                return;
            }
            foreach (var modifiedStat in BuffToApply.ModifiedStats)
            {
                BuffToApply.Id = target.AlterateStat(modifiedStat, value => value * BuffToApply.ModifierValue, 1);
            }
            target.AddBuffEffect(BuffToApply);
        }

        public void ApplyAlterationTo(Entity target, BuffEffect alterationToApply)
        {
            AlterationToApply = new BuffEffect();
            AlterationToApply = alterationToApply;
            var existingAlt = target.Alterations.FirstOrDefault(a => a.State.ToString() == alterationToApply.State.ToString());
            if (existingAlt != null)
            {
                existingAlt.ResetDuration();
                return;
            }
            target.AddAlteration(AlterationToApply);
        }
        
        private void InitializeEffectDatabase()
        {
            EffectDatabase = new Dictionary<string, BuffEffect>()
            {
                //Buffs
                {EffectType.ATKBUFF.ToString(),new BuffEffect(3,Item.AttributeStat.Attack,1.5f,"ATKBUFF","Increase attack")},
                {EffectType.DEFENSEBUFF.ToString(), new BuffEffect(3,new List<Item.AttributeStat>
                    {   Item.AttributeStat.PhysicalDefense, 
                        Item.AttributeStat.MagicalDefense
                    },
                    1.5f,"DEFENSEBUFF","Increase defense")
                },
                {Item.AttributeStat.CritRate.ToString(),new BuffEffect(3, Item.AttributeStat.CritRate,1.5f,"CritRate","Increase crit rate")},
                {Item.AttributeStat.CritDmg.ToString(), new BuffEffect(3,Item.AttributeStat.CritDmg,1.5f,"CritDmg","Increase crit damage")},
                
                //Alterations
                {AlterationState.Silence.ToString(), new BuffEffect(3,AlterationState.Silence, "Silence")},
                {AlterationState.Stun.ToString(), new BuffEffect(3,AlterationState.Stun, "Stun")},
                {AlterationState.DemonicMark.ToString(), new BuffEffect(3, AlterationState.DemonicMark, "DemonicMark")},
                {AlterationState.None.ToString(), new BuffEffect()},
                
                //Debuff
                {EffectType.BREAKDEFENSE.ToString(), new BuffEffect(3,new List<Item.AttributeStat>
                    {
                    Item.AttributeStat.MagicalDefense,
                    Item.AttributeStat.PhysicalDefense
                    },
                    0.7f,"BREAKDEFENSE","Decrease defense")
                },
                {EffectType.BREAKATTACK.ToString(),new BuffEffect(3,Item.AttributeStat.Attack,0.7f,"BREAKATTACK","Decrease attack")}
            };
        }

    }
