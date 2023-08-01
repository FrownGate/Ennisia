using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        Entity enemy = BattleSystem.Enemies[BattleSystem.EnemyPlayingID];

        enemy.UpdateEffects();
        BattleSystem.ResetSelectedEnemies();
        BattleSystem.ToggleSkills(false);

        BattleSystem.DialogueText.text = "Enemy " + BattleSystem.EnemyPlayingID + "turn";
        BattleSystem.Enemies[BattleSystem.EnemyPlayingID].AtkBar = 0;

        UseSkill(enemy.Skills[AI.ChooseSkill(BattleSystem, enemy, true)]);

        yield return new WaitForSeconds(1f);

        if (BattleSystem.Player.IsDead)
        {
            BattleSystem.SetState(new Lost(BattleSystem));
        }
        else
        {
            BattleSystem.SetState(new CheckTurn(BattleSystem));
        }
        BattleSystem.Turn += 1;
    }

    public void UseSkill(Skill skillUsed)
    {
        Entity enemy = BattleSystem.Enemies[BattleSystem.EnemyPlayingID];
        float totalDamage = 0;
        //IN ENEMY SKILL !!! TARGETS[0] IS THE PLAYER
        //THE PLAYER IN ENEMY SKILL IS THE ENEMY USING THE SKILL !!!

        List<Entity> Targets = new() { BattleSystem.Player };
        List<Entity> Allies = new() { BattleSystem.Player };

        //TODO -> use Targets & Allies Lists
        //foreach (Entity entity in BattleSystem.Enemies) { allTargets.Add(entity); }

        foreach (var skill in enemy.Skills)
        {
            skill.PassiveBeforeAttack(Targets, enemy, BattleSystem.Turn, Allies);
        }

        totalDamage += skillUsed.SkillBeforeUse(Targets, enemy, BattleSystem.Turn, Allies);
        totalDamage += skillUsed.Use(Targets, enemy, BattleSystem.Turn, Allies);
        totalDamage += skillUsed.AdditionalDamage(Targets, enemy, BattleSystem.Turn, totalDamage, Allies);

        foreach(Skill skills in BattleSystem.Player.Skills)
        {
            skills.UseIfAttacked(Targets, enemy, BattleSystem.Player, BattleSystem.Turn, totalDamage, Allies);
        }

        skillUsed.SkillAfterDamage(Targets, enemy, BattleSystem.Turn, totalDamage, Allies);

        foreach (var skill in enemy.Skills)
        {
            skill.PassiveAfterAttack(Targets, enemy, BattleSystem.Turn, totalDamage, Allies);
        }
    }
}