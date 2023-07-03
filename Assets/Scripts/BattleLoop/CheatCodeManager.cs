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
    }

    public class CheatCodeManager
    {
        private BattleSystem battleSystem;
        public static readonly Lazy<CheatCodeManager> lazy = new(() => new());
        private readonly List<CheatCodeData> cheatCodes;
        public HashSet<CheatCode> activeCheatCodes;

        public CheatCodeManager()
        {
            activeCheatCodes = new();
            cheatCodes = new()
            {
                new CheatCodeData("unkillable","poweroverwhelming", ActivateUnkillable, CheatCode.Unkillable),
                new CheatCodeData("nocd","whosyourdaddy", ActiveNoCooldown, CheatCode.NoCooldown),
            };
            battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();

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
                        cheatCode.effect();
                        activeCheatCodes.Add(cheatCode.cheatCode);
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


    }
    // FIXME: This is examples :: DO NOT REMOVE
    // ModifierID modifierId = BattleSystem.Player.Stats[Attribute.HP].AddModifier((value) => value + 1000);
    // si beosin de remove 
    // BattleSystem.Player.Stats[Attribute.HP].RemoveModifier(modifierId);
}