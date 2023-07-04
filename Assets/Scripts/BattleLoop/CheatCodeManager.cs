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

        public CheatCodeData(string key, string alternateKey, Action effect, /*Action remove,*/ CheatCode cheatCode)
        {
            this.activationKey = key;
            this.alternateKey = alternateKey;
            this.effect = effect;
            // this.remove = remove;
            this.cheatCode = cheatCode;
        }
    }
    public enum CheatCode
    {
        Unkillable,
        NoCooldown,
        FullLife,
        Purify,
        Silence,
        Stun,
        DemonicMark,
        SupportSilence,
        BreakAttack,
        BreakDefense,
        AttackUnlimited,
        Victory,
        Defeat

    }

    public class CheatCodeManager
    {
        private BattleSystem _battleInstance;
        public static Lazy<CheatCodeManager> lazy = new(() => new CheatCodeManager(null));
        private readonly List<CheatCodeData> cheatCodes;
        public HashSet<CheatCode> activeCheatCodes;

        public CheatCodeManager(BattleSystem battleSystem)
        {
            activeCheatCodes = new();
            cheatCodes = new()
            {
                new CheatCodeData("unkillable","PowerOverwhelming", ActivateUnkillable, CheatCode.Unkillable),
                new CheatCodeData("nocd","IAmIronMan", ActiveNoCooldown, CheatCode.NoCooldown),
                new CheatCodeData("fulllife","thereisnocowlevel", ActivateFullLife, CheatCode.FullLife),
                new CheatCodeData("purify","", ActivatePurify, CheatCode.Purify),
                new CheatCodeData("silence","StayAwhileAndListen", ActivateSilence, CheatCode.Silence),
                new CheatCodeData("stun","stun", ActivateStun, CheatCode.Stun),
                new CheatCodeData("demonicmark","gotthedemoninme", ActivateDemonicMark, CheatCode.DemonicMark),
                new CheatCodeData("supportsilence","junglerisbetter", ActivateSupportSilence, CheatCode.SupportSilence),
                new CheatCodeData("breakattack","breakattack", ActivateBreakAttack, CheatCode.BreakAttack),
                new CheatCodeData("breakdefense","breakdefense", ActivateBreakDefense, CheatCode.BreakDefense),
                new CheatCodeData("attackunlimited","ICanDoThisAllDay", ActivateAttackUnlimited, CheatCode.AttackUnlimited),
                new CheatCodeData("victory","WhatIsBestInLife", ActivateVictory, CheatCode.Victory),
                new CheatCodeData("defeat","IllBeBack", ActivateDefeat, CheatCode.Defeat)



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

        public void RemoveCheatCode(CheatCode cheatCode)
        {
            // TODO: Remove effects of cheat code

            if (cheatCodes.Find(x => x.cheatCode == cheatCode).remove != null) cheatCodes.Find(x => x.cheatCode == cheatCode).remove();
            activeCheatCodes.Remove(cheatCode);

        }

        private void ActivateUnkillable()
        {
            // Apply cheat effect for god mode
            Debug.LogWarning("Unkillable activated");
        }
        private void ActiveNoCooldown()
        {

            Debug.LogWarning("No cooldown activated");
        }
        private void ActivateFullLife()
        {
            // Apply cheat effect for full life

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
            // TODO: Apply cheat effect for silence

            // silence for 2 turns the player
            // _battleInstance.Player.ApplyEffect(new Effect.AlterationState(2, Effect.AlterationState.Silence));
            Debug.LogWarning("Silence activated");
        }
        private void ActivateStun()
        {
            // TODO: Apply cheat effect for stun
            Debug.LogWarning("Stun activated");
        }
        private void ActivateDemonicMark()
        {
            // TODO: Apply cheat effect for demonic mark
            Debug.LogWarning("Demonic mark activated");
        }
        private void ActivateSupportSilence()
        {
            // TODO: Apply cheat effect for support silence
            Debug.LogWarning("Support silence activated");
        }
        private void ActivateBreakAttack()
        {
            // TODO: Apply cheat effect for break attack
            Debug.LogWarning("Break attack activated");
        }
        private void ActivateBreakDefense()
        {
            // TODO: Apply cheat effect for break defense
            Debug.LogWarning("Break defense activated");
        }
        private void ActivateAttackUnlimited()
        {
            // TODO: Apply cheat effect for attack unlimited
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




    }
    // FIXME: This is examples :: DO NOT REMOVE
    // ModifierID modifierId = BattleSystem.Player.Stats[Attribute.HP].AddModifier((value) => value + 1000);
    // si beosin de remove 
    // BattleSystem.Player.Stats[Attribute.HP].RemoveModifier(modifierId);
}