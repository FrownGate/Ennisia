using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoBattle : State
{
    public AutoBattle(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        while (!BattleSystem.IsBattleOver())
        {
            BattleSystem.SetState(new WhoGoFirst(BattleSystem));
            AISelectSkillAndEnemy();
            yield return new WaitForSeconds(1);
        }

        BattleSystem.SetState(BattleSystem.PlayerHasWin ? new Won(BattleSystem) : new Lost(BattleSystem));
    }

    private void AISelectSkillAndEnemy()
    {
        if (!BattleSystem.Player.IsDead && BattleSystem.Player.Skills.Any())
        {

            int selectedSkillIndex = AI.ChooseSkill(BattleSystem, BattleSystem.Player, false);
            BattleSystem.SetState(new SelectSpell(BattleSystem, selectedSkillIndex));
            //copy paste spell selecyion
            if (BattleSystem.Enemies.Any())
            {
                int selectedEnemyIndex = BattleSystem.Enemies.IndexOf(AI.FindBestEnnemy(BattleSystem.Enemies));//need to random between bdef ennemy or lowest ennemy
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