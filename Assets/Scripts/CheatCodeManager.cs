using System.Collections.Generic;
using UnityEngine;
using System;

namespace CheatCodeNS
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
        // ALLY
        Unkillable,
        NoCooldown,
        FullLife,
        Life25,
        Life50,
        Life75,
        Purify,
        SilenceEffect,
        StunEffect,
        TauntEffect,
        DemonicMarkEffect,
        ImmunityBuff,
        SupportSilenceEffect,
        BreakAttackDebuff,
        BreakDefenseDebuff,
        CritDamageBuff,
        CritRateBuff,
        AttackBuff,
        // ENEMY
        EnemyLife25,
        EnemyLife50,
        EnemyLife75,
        EnemyPurify,
        EnemySilenceEffect,
        EnemyStunEffect,
        EnemyTauntEffect,
        EnemyDemonicMarkEffect,
        EnemyImmunityBuff,
        EnemySupportSilenceEffect,
        EnemyBreakAttackDebuff,
        EnemyBreakDefenseDebuff,
        EnemyCritDamageBuff,
        EnemyCritRateBuff,
        EnemyAttackBuff,
        AttackUnlimited,


        Victory,
        Defeat,
        CleanWave
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
                new CheatCodeData("life25","life25", ActivateLife25, null, CheatCode.Life25),
                new CheatCodeData("life50","life50", ActivateLife50, null, CheatCode.Life50),
                new CheatCodeData("life75","life75", ActivateLife75, null, CheatCode.Life75),
                new CheatCodeData("purify","", ActivatePurify, null, CheatCode.Purify),
                new CheatCodeData("silence","StayAwhileAndListen", ActivateSilence, null, CheatCode.SilenceEffect),
                new CheatCodeData("stun","stun", ActivateStun, null, CheatCode.StunEffect),
                new CheatCodeData("taunt","taunt", ActivateTaunt, null, CheatCode.TauntEffect),
                new CheatCodeData("demonicmark","gotthedemoninme", ActivateDemonicMark, null, CheatCode.DemonicMarkEffect),
                new CheatCodeData("suppsilence","junglerisbetter", ActivateSupportSilence, null, CheatCode.SupportSilenceEffect),
                new CheatCodeData("immunity","immunitybuff", ActivateImmunityBuff, null, CheatCode.ImmunityBuff),
                new CheatCodeData("breakatk","breakattackdebuff", ActivateBreakAttackDebuff, null, CheatCode.BreakAttackDebuff),
                new CheatCodeData("breakdeff","breakdefensedebuff", ActivateBreakDefenseDebuff, null, CheatCode.BreakDefenseDebuff),
                new CheatCodeData("critdmg","critdamagebuff", ActivateCritDamageBuff, null, CheatCode.CritDamageBuff),
                new CheatCodeData("critrate","critratebuff", ActivateCritRateBuff, null, CheatCode.CritRateBuff),
                new CheatCodeData("atkbuff","attackbuff", ActivateAttackBuff, null, CheatCode.AttackBuff),
                new CheatCodeData("attackunlimited","ICanDoThisAllDay", ActivateAttackUnlimited, RemoveAttackUnlimited, CheatCode.AttackUnlimited),
                new CheatCodeData("enemylife25","enemylife25", ActivateEnemyLife25, null, CheatCode.EnemyLife25),
                new CheatCodeData("enemylife50","enemylife50", ActivateEnemyLife50, null, CheatCode.EnemyLife50),
                new CheatCodeData("enemylife75","enemylife75", ActivateEnemyLife75, null, CheatCode.EnemyLife75),
                new CheatCodeData("enemypurify","enemypurify", ActivateEnemyPurify, null, CheatCode.EnemyPurify),
                new CheatCodeData("enemysilence","enemysilence", ActivateEnemySilence, null, CheatCode.EnemySilenceEffect),
                new CheatCodeData("enemystun","enemystun", ActivateEnemyStun, null, CheatCode.EnemyStunEffect),
                new CheatCodeData("enemytaunt","enemytaunt", ActivateEnemyTaunt, null, CheatCode.EnemyTauntEffect),
                new CheatCodeData("enemydemonicmark","enemydemonicmark", ActivateEnemyDemonicMark, null, CheatCode.EnemyDemonicMarkEffect),
                new CheatCodeData("enemysuppsilence","enemysuppsilence", ActivateEnemySupportSilence, null, CheatCode.EnemySupportSilenceEffect),
                new CheatCodeData("enemyimmunity","enemyimmunity", ActivateEnemyImmunityBuff, null, CheatCode.EnemyImmunityBuff),
                new CheatCodeData("enemybreakatk","enemybreakatk", ActivateEnemyBreakAttackDebuff, null, CheatCode.EnemyBreakAttackDebuff),
                new CheatCodeData("enemybreakdeff","enemybreakdeff", ActivateEnemyBreakDefenseDebuff, null, CheatCode.EnemyBreakDefenseDebuff),
                new CheatCodeData("enemycritdmg","enemycritdmg", ActivateEnemyCritDamageBuff, null, CheatCode.EnemyCritDamageBuff),
                new CheatCodeData("enemycritrate","enemycritrate", ActivateEnemyCritRateBuff, null, CheatCode.EnemyCritRateBuff),
                new CheatCodeData("enemyatkbuff","enemyatkbuff", ActivateEnemyAttackBuff, null, CheatCode.EnemyAttackBuff),
                new CheatCodeData("victory","WhatIsBestInLife", ActivateVictory, null, CheatCode.Victory),
                new CheatCodeData("defeat","IllBeBack", ActivateDefeat, null, CheatCode.Defeat),
                new CheatCodeData("cleanwave","cleanwave", ActivateCleanWave, null, CheatCode.CleanWave)
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
                    }
                    else
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
        #region Ally
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
        private void ActivateLife25()
        {
            _battleInstance.Player.CurrentHp = _battleInstance.Player.Stats[Attribute.HP].Value * 25 / 100;
            Debug.LogWarning("Life 25% activated");
            ActiveCheatCodes.Remove(CheatCode.Life25);
        }
        private void ActivateLife50()
        {
            _battleInstance.Player.CurrentHp = _battleInstance.Player.Stats[Attribute.HP].Value * 50 / 100;
            Debug.LogWarning("Life 50% activated");
            ActiveCheatCodes.Remove(CheatCode.Life50);

        }
        private void ActivateLife75()
        {
            _battleInstance.Player.CurrentHp = _battleInstance.Player.Stats[Attribute.HP].Value * 75 / 100;
            Debug.LogWarning("Life 75% activated");
            ActiveCheatCodes.Remove(CheatCode.Life75);

        }
        private void ActivatePurify()
        {
            _battleInstance.Player.RemoveAlterations();
            Debug.LogWarning("Purify activated");
            ActiveCheatCodes.Remove(CheatCode.Purify);
            Debug.Log(_battleInstance.Player.Effects.Count);

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
        private void ActivateTaunt()
        {
            _battleInstance.Player.ApplyEffect(new Taunt(2));
            Debug.LogWarning("Taunt activated");
            ActiveCheatCodes.Remove(CheatCode.TauntEffect);
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
        private void ActivateImmunityBuff()
        {
            _battleInstance.Player.ApplyEffect(new Immunity(2));
            Debug.LogWarning("Immunity buff activated");
            ActiveCheatCodes.Remove(CheatCode.ImmunityBuff);

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

        #endregion

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
        private void ActivateCleanWave()
        {
            _battleInstance.EndWave(true);
            Debug.LogWarning("Clean wave activated");
            ActiveCheatCodes.Remove(CheatCode.CleanWave);
        }

        #region Enemy

        private void ActivateEnemyLife25()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.CurrentHp = enemy.Stats[Attribute.HP].Value * 25 / 100;
            }
            Debug.LogWarning("Enemy life 25% activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyLife25);
        }

        private void ActivateEnemyLife50()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.CurrentHp = enemy.Stats[Attribute.HP].Value * 50 / 100;
            }
            Debug.LogWarning("Enemy life 50% activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyLife50);
        }

        private void ActivateEnemyLife75()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.CurrentHp = enemy.Stats[Attribute.HP].Value * 75 / 100;
            }
            Debug.LogWarning("Enemy life 75% activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyLife75);
        }

        private void ActivateEnemyPurify()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.RemoveAlterations();
            }
            Debug.LogWarning("Enemy purify activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyPurify);
        }

        private void ActivateEnemySilence()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new Silence(2));
            }
            Debug.LogWarning("Enemy silence activated");
            ActiveCheatCodes.Remove(CheatCode.EnemySilenceEffect);
        }

        private void ActivateEnemyStun()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new Stun(2));
            }
            Debug.LogWarning("Enemy stun activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyStunEffect);
        }

        private void ActivateEnemyTaunt()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new Taunt(2));
            }
            Debug.LogWarning("Enemy taunt activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyTauntEffect);
        }

        private void ActivateEnemyDemonicMark()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new DemonicMark(), 4);
            }
            Debug.LogWarning("Enemy demonic mark activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyDemonicMarkEffect);
        }

        private void ActivateEnemySupportSilence()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new SupportSilence(2));
            }
            Debug.LogWarning("Enemy support silence activated");
            ActiveCheatCodes.Remove(CheatCode.EnemySupportSilenceEffect);
        }

        private void ActivateEnemyImmunityBuff()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new Immunity(2));
            }
            Debug.LogWarning("Enemy immunity buff activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyImmunityBuff);
        }

        private void ActivateEnemyBreakAttackDebuff()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new BreakAttack(2));
            }
            Debug.LogWarning("Enemy break attack activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyBreakAttackDebuff);
        }

        private void ActivateEnemyBreakDefenseDebuff()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new BreakDefense(2));
            }
            Debug.LogWarning("Enemy break defense activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyBreakDefenseDebuff);
        }

        private void ActivateEnemyCritDamageBuff()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new CritDmgBuff(2));
            }
            Debug.LogWarning("Enemy crit damage buff activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyCritDamageBuff);
        }

        private void ActivateEnemyCritRateBuff()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new CritRateBuff(2));
            }
            Debug.LogWarning("Enemy crit rate buff activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyCritRateBuff);
        }

        private void ActivateEnemyAttackBuff()
        {
            foreach (Entity enemy in _battleInstance.Enemies)
            {
                enemy.ApplyEffect(new AttackBuff(2));
            }
            Debug.LogWarning("Enemy attack buff activated");
            ActiveCheatCodes.Remove(CheatCode.EnemyAttackBuff);
        }



        #endregion

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
}
// FIXME: This is examples :: DO NOT REMOVE
// ModifierID modifierId = BattleSystem.Player.Stats[Attribute.HP].AddModifier((value) => value + 1000);
// si beosin de remove 
// BattleSystem.Player.Stats[Attribute.HP].RemoveModifier(modifierId);

// _battleInstance.Player.Skills.Where(x => x.Cooldown > 0).ToList().ForEach(x => x.Cooldown = 0);