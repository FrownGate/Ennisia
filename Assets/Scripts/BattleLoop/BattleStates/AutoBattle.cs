using System.Collections;
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
            int selectedSkillIndex = Random.Range(0, BattleSystem.Player.Skills.Count);
            BattleSystem.SetState(new SelectSpell(BattleSystem, selectedSkillIndex));
            //copy paste spell selecyion
            if (BattleSystem.Enemies.Any())
            {


                int selectedEnemyIndex = FindLowestEnemy();//need to random between bdef ennemy or lowest ennemy
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
    private int FindLowestEnemy()
    {
        int id = 0;
        Entity lowestEnemy = null;

        foreach(Entity enemy in BattleSystem.Enemies)
        {
            if(lowestEnemy == null ||enemy.CurrentHp < lowestEnemy.CurrentHp)
            {
                lowestEnemy = enemy;
                id = BattleSystem.Enemies.IndexOf(enemy);
            }
        }
        return id;

    }
}