
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    
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
                {"AttackBuff",new BuffEffect(3,Item.AttributeStat.Attack,1.5f)},
                {"DefenseBuff", new BuffEffect(3,new List<Item.AttributeStat>
                    {   Item.AttributeStat.PhysicalDefense, 
                        Item.AttributeStat.MagicalDefense
                    },
                    1.5f)
                },
                {"CritRate",new BuffEffect(3, Item.AttributeStat.CritRate,1.5f)},
                {"CritDamage", new BuffEffect(3,Item.AttributeStat.CritDmg,1.5f)},
                
                //Alterations
                {"Silence", new BuffEffect(3,AlterationState.Silence)},
                {"Stun", new BuffEffect(3,AlterationState.Stun)},
                {"SupportSilence", new BuffEffect()},
                
                //Debuff
                {"BreakDefense", new BuffEffect(3,new List<Item.AttributeStat>
                    {
                    Item.AttributeStat.MagicalDefense,
                    Item.AttributeStat.PhysicalDefense
                    },
                    0.7f)
                },
                {"BreakAttack",new BuffEffect(3,Item.AttributeStat.Attack,0.7f)}
            };
        }

    }
