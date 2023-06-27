﻿using System.Collections;
using System.Linq;
using UnityEngine;

public class AutoBattle : State
{
    public AutoBattle(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.SetState(new WhoGoFirst(BattleSystem));
        while (!BattleSystem.IsBattleOver())
        {
            BattleSystem.SetState(new CheckTurn(BattleSystem));
            //AISelectSkillAndEnemy();
            yield return new WaitForSeconds(1);
        }

        BattleSystem.SetState(BattleSystem.PlayerHasWin ? new Won(BattleSystem) : new Lost(BattleSystem));
    }

    private void AISelectSkillAndEnemy()
    {
        if (!BattleSystem.PlayerIsDead() && BattleSystem.Player.Skills.Any())
        {
            int selectedSkillIndex = Random.Range(0, BattleSystem.Player.Skills.Count);
            BattleSystem.SetState(new SelectSpell(BattleSystem, selectedSkillIndex));

            if (BattleSystem.Enemies.Any())
            {
                int selectedEnemyIndex = Random.Range(0, BattleSystem.Enemies.Count);
                BattleSystem.Enemies[selectedEnemyIndex].HaveBeenSelected();
                BattleSystem.GetSelectedEnemies(BattleSystem.Enemies);
                BattleSystem.StartCoroutine(new SelectTarget(BattleSystem, BattleSystem.GetSelectedSkill(selectedSkillIndex)).Attack());
            }
            else
            {
                Debug.LogWarning("AI has no enemies to select.");
            }
        }
        else
        {
            Debug.LogWarning("AI has no skills or allies to select.");
        }
    }
}