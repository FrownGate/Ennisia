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
                new CheatCodeData("unkillable","poweroverwhelming", ActivateUnkillable, CheatCode.Unkillable),
                new CheatCodeData("nocd","whosyourdaddy", ActiveNoCooldown, CheatCode.NoCooldown),
                new CheatCodeData("fulllife","thereisnocowlevel", ActivateFullLife, CheatCode.FullLife),
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


    }
    // FIXME: This is examples :: DO NOT REMOVE
    // ModifierID modifierId = BattleSystem.Player.Stats[Attribute.HP].AddModifier((value) => value + 1000);
    // si beosin de remove 
    // BattleSystem.Player.Stats[Attribute.HP].RemoveModifier(modifierId);
}