using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace CheatCode
{
    public class CheatCodeData
    {
        internal string activationKey;
        internal string alternateKey;
        internal Action effect;
        internal Action remove;
        internal CheatCode cheatCode;

        public CheatCodeData(string key, string alternateKey, Action effect, Action remove, CheatCode cheatCode)
        {
            this.activationKey = key;
            this.alternateKey = alternateKey;
            this.effect = effect;
            this.remove = remove;
            this.cheatCode = cheatCode;
        }
    }
    public enum CheatCode
    {
        Unkillable,
        NoCooldown,
        FullLife,
        Purify,
        SilenceEffect,
        StunEffect,
        DemonicMarkEffect,
        SupportSilenceEffect,
        BreakAttackDebuff,
        BreakDefenseDebuff,
        CritDamageBuff,
        CritRateBuff,
        AttackBuff,
        AttackUnlimited,
        Victory,
        Defeat

    }

    public class CheatCodeManager
    {
        private readonly BattleSystem _battleInstance;
        public static Lazy<CheatCodeManager> lazy = new(() => new CheatCodeManager(null));
        private readonly List<CheatCodeData> cheatCodes;
        public HashSet<CheatCode> activeCheatCodes;
        private readonly HashSet<ModifierID> _modifiers = new();

        public CheatCodeManager(BattleSystem battleSystem)
        {
            activeCheatCodes = new();
            cheatCodes = new()
            {
                new CheatCodeData("unkillable","PowerOverwhelming", ActivateUnkillable, RemoveUnkillable, CheatCode.Unkillable),
                new CheatCodeData("nocd","IAmIronMan", ActiveNoCooldown, RemoveNoCooldown, CheatCode.NoCooldown),
                new CheatCodeData("fulllife","thereisnocowlevel", ActivateFullLife, null, CheatCode.FullLife),
                new CheatCodeData("purify","", ActivatePurify, null, CheatCode.Purify),
                new CheatCodeData("silence","StayAwhileAndListen", ActivateSilence, null, CheatCode.SilenceEffect),
                new CheatCodeData("stun","stun", ActivateStun, null, CheatCode.StunEffect),
                new CheatCodeData("demonicmark","gotthedemoninme", ActivateDemonicMark, null, CheatCode.DemonicMarkEffect),
                new CheatCodeData("suppsilence","junglerisbetter", ActivateSupportSilence, null, CheatCode.SupportSilenceEffect),
                new CheatCodeData("breakatk","breakattackdebuff", ActivateBreakAttackDebuff, null, CheatCode.BreakAttackDebuff),
                new CheatCodeData("breakdeff","breakdefensedebuff", ActivateBreakDefenseDebuff, null, CheatCode.BreakDefenseDebuff),
                new CheatCodeData("critdmg","critdamagebuff", ActivateCritDamageBuff, null, CheatCode.CritDamageBuff),
                new CheatCodeData("critrate","critratebuff", ActivateCritRateBuff, null, CheatCode.CritRateBuff),
                new CheatCodeData("atkbuff","attackbuff", ActivateAttackBuff, null, CheatCode.AttackBuff),
                new CheatCodeData("attackunlimited","ICanDoThisAllDay", ActivateAttackUnlimited, RemoveAttackUnlimited, CheatCode.AttackUnlimited),
                new CheatCodeData("victory","WhatIsBestInLife", ActivateVictory, null, CheatCode.Victory),
                new CheatCodeData("defeat","IllBeBack", ActivateDefeat, null, CheatCode.Defeat)



            };
            _battleInstance = battleSystem;

        }

        public void CheckAndActivateCheat(string input)
        {

            input = input.ToLower();
            foreach (CheatCodeData cheatCode in cheatCodes)
            {
                if (input == cheatCode.activationKey || input == cheatCode.alternateKey)
                {
                    if (!activeCheatCodes.Contains(cheatCode.cheatCode))
                    {
                        activeCheatCodes.Add(cheatCode.cheatCode);
                        cheatCode.effect();
                    }
                    else
                    {
                        Debug.LogWarning("Cheat code already activated");
                    }
                }
            }
        }
        public void CheckAndRemoveCheat(string input)
        {

            input = input.ToLower();
            foreach (CheatCodeData cheatCode in cheatCodes)
            {
                if (input == cheatCode.activationKey || input == cheatCode.alternateKey)
                {
                    if (activeCheatCodes.Contains(cheatCode.cheatCode))
                    {
                        cheatCode.remove();
                    }
                    else
                    {
                        Debug.LogWarning("Cheat code is not yet activated");
                    }
                }
            }
        }

        private void ActivateUnkillable()
        {
            Debug.LogWarning("Unkillable activated");
        }
        private void ActiveNoCooldown()
        {

            Debug.LogWarning("No cooldown activated");
        }
        private void ActivateFullLife()
        {
            _battleInstance.Player.CurrentHp = _battleInstance.Player.Stats[Attribute.HP].Value;
            Debug.LogWarning("Full life activated");
            activeCheatCodes.Remove(CheatCode.FullLife);

        }
        private void ActivatePurify()
        {
            // TODO: Apply cheat effect for purify
            Debug.LogWarning("Purify activated");
        }
        private void ActivateSilence()
        {
            _battleInstance.Player.ApplyEffect(new Silence(2));
            Debug.LogWarning("Silence activated");
        }
        private void ActivateStun()
        {
            _battleInstance.Player.ApplyEffect(new Stun(2));
            Debug.LogWarning("Stun activated");
        }
        private void ActivateDemonicMark()
        {
            _battleInstance.Player.ApplyEffect(new DemonicMark(2));
            Debug.LogWarning("Demonic mark activated");
        }
        private void ActivateSupportSilence()
        {
            _battleInstance.Player.ApplyEffect(new SupportSilence(2));
            Debug.LogWarning("Support silence activated");
        }
        private void ActivateBreakAttackDebuff()
        {
            _battleInstance.Player.ApplyEffect(new BreakAttack(2));
            Debug.LogWarning("Break attack activated");
        }
        private void ActivateBreakDefenseDebuff()
        {
            _battleInstance.Player.ApplyEffect(new BreakDefense(2));
            Debug.LogWarning("Break defense activated");
        }
        private void ActivateCritDamageBuff()
        {
            _battleInstance.Player.ApplyEffect(new CritDmgBuff(2));
            Debug.LogWarning("Crit damage buff activated");
        }
        private void ActivateCritRateBuff()
        {
            _battleInstance.Player.ApplyEffect(new CritRateBuff(2));
            Debug.LogWarning("Crit rate buff activated");
        }
        private void ActivateAttackBuff()
        {
            _battleInstance.Player.ApplyEffect(new AttackBuff(2));
            Debug.LogWarning("Attack buff activated");
        }

        private void ActivateAttackUnlimited()
        {
            _modifiers.Add(_battleInstance.Player.Stats[Attribute.Attack].AddModifier((value) => value + 10000));
            _modifiers.Add(_battleInstance.Player.Stats[Attribute.PhysicalDamages].AddModifier((value) => value + 10000));
            _modifiers.Add(_battleInstance.Player.Stats[Attribute.MagicalDamages].AddModifier((value) => value + 10000));

            Debug.LogWarning("Attack unlimited activated");
        }
        private void ActivateVictory()
        {
            _battleInstance.SetState(new Won(_battleInstance));
            Debug.LogWarning("Victory activated");
            activeCheatCodes.Remove(CheatCode.Victory);
        }
        private void ActivateDefeat()
        {
            _battleInstance.SetState(new Lost(_battleInstance));
            Debug.LogWarning("Defeat activated");
            activeCheatCodes.Remove(CheatCode.Defeat);
        }

        // REMOVERS
        private void RemoveUnkillable()
        {
            activeCheatCodes.Remove(CheatCode.Unkillable);
            Debug.LogWarning("Unkillable removed");
        }

        private void RemoveNoCooldown()
        {
            activeCheatCodes.Remove(CheatCode.NoCooldown);
            Debug.LogWarning("No cooldown removed");
        }

        private void RemoveAttackUnlimited()
        {
            foreach (ModifierID modifier in _modifiers)
            {
                _battleInstance.Player.Stats[Attribute.Attack].RemoveModifier(modifier);
                _battleInstance.Player.Stats[Attribute.PhysicalDamages].RemoveModifier(modifier);
                _battleInstance.Player.Stats[Attribute.MagicalDamages].RemoveModifier(modifier);
            }
            _modifiers.Clear();
            activeCheatCodes.Remove(CheatCode.AttackUnlimited);
            Debug.LogWarning("Attack unlimited removed");
        }




    }
    // FIXME: This is examples :: DO NOT REMOVE
    // ModifierID modifierId = BattleSystem.Player.Stats[Attribute.HP].AddModifier((value) => value + 1000);
    // si beosin de remove 
    // BattleSystem.Player.Stats[Attribute.HP].RemoveModifier(modifierId);

    // _battleInstance.Player.Skills.Where(x => x.Cooldown > 0).ToList().ForEach(x => x.Cooldown = 0);

}