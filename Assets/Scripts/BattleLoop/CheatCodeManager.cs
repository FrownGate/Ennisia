using System.Collections.Generic;
using UnityEngine;
using System;

namespace CheatCode
{
    public class CheatCodeData
    {
        internal string _activationKey;
        internal string _alternateKey;
        internal Action _effect;
        internal Action _remove;
        internal CheatCode _cheatCode;

        public CheatCodeData(string key, string alternateKey, Action effect, Action remove, CheatCode cheatCode)
        {
            _activationKey = key;
            _alternateKey = alternateKey;
            _effect = effect;
            _remove = remove;
            _cheatCode = cheatCode;
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
        public static Lazy<CheatCodeManager> Lazy = new(() => new CheatCodeManager(null));
        private readonly List<CheatCodeData> _cheatCodes;
        public HashSet<CheatCode> ActiveCheatCodes;
        private readonly HashSet<ModifierID> _modifiers = new();

        public CheatCodeManager(BattleSystem battleSystem)
        {
            ActiveCheatCodes = new();
            _cheatCodes = new()
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
            foreach (CheatCodeData cheatCode in _cheatCodes)
            {
                if (input == cheatCode._activationKey || input == cheatCode._alternateKey)
                {
                    if (!ActiveCheatCodes.Contains(cheatCode._cheatCode))
                    {
                        ActiveCheatCodes.Add(cheatCode._cheatCode);
                        cheatCode._effect();
                        return;
                    }
                    else
                    {
                        Debug.LogWarning("Cheat code already activated");
                        return;
                    }
                }
            }
        }
        public void CheckAndRemoveCheat(string input)
        {
            input = input.ToLower();
            foreach (CheatCodeData cheatCode in _cheatCodes)
            {
                if (input == cheatCode._activationKey || input == cheatCode._alternateKey)
                {
                    if (ActiveCheatCodes.Contains(cheatCode._cheatCode))
                    {
                        cheatCode._remove();
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
            ActiveCheatCodes.Remove(CheatCode.FullLife);

        }
        private void ActivatePurify()
        {
            // TODO: Apply cheat effect for purify
            Debug.LogWarning("Purify activated");
            ActiveCheatCodes.Remove(CheatCode.Purify);

        }
        private void ActivateSilence()
        {
            _battleInstance.Player.ApplyEffect(new Silence(2));
            Debug.LogWarning("Silence activated");
            ActiveCheatCodes.Remove(CheatCode.SilenceEffect);

        }
        private void ActivateStun()
        {
            _battleInstance.Player.ApplyEffect(new Stun(2));
            Debug.LogWarning("Stun activated");
            ActiveCheatCodes.Remove(CheatCode.StunEffect);

        }
        private void ActivateDemonicMark()
        {
            _battleInstance.Player.ApplyEffect(new DemonicMark(), 4);
            Debug.LogWarning("Demonic mark activated");
            ActiveCheatCodes.Remove(CheatCode.DemonicMarkEffect);
        }
        private void ActivateSupportSilence()
        {
            _battleInstance.Player.ApplyEffect(new SupportSilence(2));
            Debug.LogWarning("Support silence activated");
            ActiveCheatCodes.Remove(CheatCode.SupportSilenceEffect);

        }
        private void ActivateBreakAttackDebuff()
        {
            _battleInstance.Player.ApplyEffect(new BreakAttack(2));
            Debug.LogWarning("Break attack activated");
            ActiveCheatCodes.Remove(CheatCode.BreakAttackDebuff);

        }
        private void ActivateBreakDefenseDebuff()
        {
            _battleInstance.Player.ApplyEffect(new BreakDefense(2));
            Debug.LogWarning("Break defense activated");
            ActiveCheatCodes.Remove(CheatCode.BreakDefenseDebuff);

        }
        private void ActivateCritDamageBuff()
        {
            _battleInstance.Player.ApplyEffect(new CritDmgBuff(2));
            Debug.LogWarning("Crit damage buff activated");
            ActiveCheatCodes.Remove(CheatCode.CritDamageBuff);

        }
        private void ActivateCritRateBuff()
        {
            _battleInstance.Player.ApplyEffect(new CritRateBuff(2));
            Debug.LogWarning("Crit rate buff activated");
            ActiveCheatCodes.Remove(CheatCode.CritRateBuff);

        }
        private void ActivateAttackBuff()
        {
            _battleInstance.Player.ApplyEffect(new AttackBuff(2));
            Debug.LogWarning("Attack buff activated");
            ActiveCheatCodes.Remove(CheatCode.AttackBuff);

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
            ActiveCheatCodes.Remove(CheatCode.Victory);
        }
        private void ActivateDefeat()
        {
            _battleInstance.SetState(new Lost(_battleInstance));
            Debug.LogWarning("Defeat activated");
            ActiveCheatCodes.Remove(CheatCode.Defeat);
        }

        // REMOVERS
        private void RemoveUnkillable()
        {
            ActiveCheatCodes.Remove(CheatCode.Unkillable);
            Debug.LogWarning("Unkillable removed");
        }

        private void RemoveNoCooldown()
        {
            ActiveCheatCodes.Remove(CheatCode.NoCooldown);
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
            ActiveCheatCodes.Remove(CheatCode.AttackUnlimited);
            Debug.LogWarning("Attack unlimited removed");
        }
    }
    // FIXME: This is examples :: DO NOT REMOVE
    // ModifierID modifierId = BattleSystem.Player.Stats[Attribute.HP].AddModifier((value) => value + 1000);
    // si beosin de remove 
    // BattleSystem.Player.Stats[Attribute.HP].RemoveModifier(modifierId);

    // _battleInstance.Player.Skills.Where(x => x.Cooldown > 0).ToList().ForEach(x => x.Cooldown = 0);

}