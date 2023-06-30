using System.Collections.Generic;
using UnityEngine;
using System;

namespace CheatCode
{
    public class CheatCodeData
    {
        internal string activationKey;
        internal string alternateKey;
        internal Action effect;
        internal CheatCode cheatCode;

        public CheatCodeData(string key, string alternateKey, Action effect, CheatCode cheatCode)
        {
            this.activationKey = key;
            this.alternateKey = alternateKey;
            this.effect = effect;
            this.cheatCode = cheatCode;
        }
    }
    public enum CheatCode
    {
        GodMode,
    }

    public class CheatCodeManager
    {
        List<CheatCodeData> cheatCodes;
        public HashSet<CheatCode> activeCheatCodes;

        public CheatCodeManager()
        {
            activeCheatCodes = new();
            cheatCodes = new()
            {
                new CheatCodeData("godmode","poweroverwhelming", ActivateGodMode, CheatCode.GodMode),
            };

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
                        Debug.Log("Cheat code already activated");
                    }
                }
            }
        }

        public void RemoveCheatCode(CheatCode cheatCode)
        {
            // TODO: Remove effects of cheat code
            activeCheatCodes.Remove(cheatCode);
        }

        private void ActivateGodMode()
        {
            // Apply cheat effect for god mode
            Debug.Log("God mode activated");
        }

        private void ActivateUnlimitedAmmo()
        {
            // Apply cheat effect for unlimited ammo
            Debug.Log("Unlimited ammo activated");
        }

        private void UnlockAllLevels()
        {
            // Apply cheat effect to unlock all levels
            Debug.Log("All levels unlocked");
        }
    }
}