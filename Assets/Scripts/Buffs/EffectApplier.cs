
    using System;
    using System.Collections.Generic;
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
            target.AddAlteration(AlterationToApply);
        }
        
        private void InitializeEffectDatabase()
        {
            EffectDatabase = new Dictionary<string, BuffEffect>()
            {
                //Buffs
                {EffectType.ATKBUFF.ToString(),new BuffEffect(3,Item.AttributeStat.Attack,1.5f)},
                {EffectType.DEFENSEBUFF.ToString(), new BuffEffect(3,new List<Item.AttributeStat>
                    {   Item.AttributeStat.PhysicalDefense, 
                        Item.AttributeStat.MagicalDefense
                    },
                    1.5f)
                },
                {Item.AttributeStat.CritRate.ToString(),new BuffEffect(3, Item.AttributeStat.CritRate,1.5f)},
                {Item.AttributeStat.CritDmg.ToString(), new BuffEffect(3,Item.AttributeStat.CritDmg,1.5f)},
                
                //Alterations
                {AlterationState.Silence.ToString(), new BuffEffect(3,AlterationState.Silence)},
                {AlterationState.Stun.ToString(), new BuffEffect(3,AlterationState.Stun)},
                {AlterationState.DemonicMark.ToString(), new BuffEffect(3, AlterationState.DemonicMark)},
                {AlterationState.None.ToString(), new BuffEffect()},
                
                //Debuff
                {EffectType.BREAKDEFENSE.ToString(), new BuffEffect(3,new List<Item.AttributeStat>
                    {
                    Item.AttributeStat.MagicalDefense,
                    Item.AttributeStat.PhysicalDefense
                    },
                    0.7f)
                },
                {EffectType.BREAKATTACK.ToString(),new BuffEffect(3,Item.AttributeStat.Attack,0.7f)}
            };
        }

    }
