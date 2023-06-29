using System.Collections.Generic;
using UnityEngine;
using System;

namespace CheatCode
{
    public enum CheatCode
    {
        UnlimitedHealth,
        NoCooldown,
        DoubleDamage,
        GodMode,
    }

    public class CheatCodeManager
    {
        private Dictionary<string, Action> cheatEffects;

        public CheatCodeManager()
        {
            cheatEffects = new Dictionary<string, Action>();

            // Map input values to cheat effects
            cheatEffects.Add("godmode", ActivateGodMode);
            cheatEffects.Add("unlimitedammo", ActivateUnlimitedAmmo);
            cheatEffects.Add("unlocklevels", UnlockAllLevels);
        }

        public void CheckInputAndActivateCheat(string input)
        {
            if (cheatEffects.TryGetValue(input.ToLower(), out var cheatEffect))
            {
                cheatEffect.Invoke();
            }
            else
            {
                Debug.LogWarning($"Invalid cheat code: {input}");
            }
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