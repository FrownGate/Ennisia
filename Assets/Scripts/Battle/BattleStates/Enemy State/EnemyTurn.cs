using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.ResetSelectedEnemies();
        BattleSystem.ToggleSkills(false);
        BattleSystem.DialogueText.text = "Enemy " + BattleSystem.EnemyPlayingID + "turn";
        BattleSystem.Enemies[BattleSystem.EnemyPlayingID].AtkBar = 0;
        Debug.Log("truc bidule = " + BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills[AI.ChooseSkill(BattleSystem, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], true)]);
        UseSkill(BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills[AI.ChooseSkill(BattleSystem,
            BattleSystem.Enemies[BattleSystem.EnemyPlayingID], true)]);

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
        Debug.Log($"Skill used : {skillUsed.Data.Name}");
        float totalDamage = 0;
        //IN ENEMY SKILL !!! TARGETS[0] IS THE PLAYER
        //THE PLAYER IN ENEMY SKILL IS THE ENEMY USING THE SKILL !!!

        List<Entity> allTargets = new() { BattleSystem.Player };

        //TODO -> use Targets & Allies Lists
        //foreach (Entity entity in BattleSystem.Enemies) { allTargets.Add(entity); }

        foreach (var skill in BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills)
        {
            skill.PassiveBeforeAttack(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn);
        }

        totalDamage += skillUsed.SkillBeforeUse(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn);
        totalDamage += skillUsed.Use(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn);
        totalDamage += skillUsed.AdditionalDamage(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn, totalDamage);

        foreach(Skill skills in BattleSystem.Player.Skills)
        {
            skills.UseIfAttacked(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Player, BattleSystem.Turn, totalDamage);
        }

        skillUsed.SkillAfterDamage(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn, totalDamage);

        foreach (var skill in BattleSystem.Enemies[BattleSystem.EnemyPlayingID].Skills)
        {
            skill.PassiveAfterAttack(allTargets, BattleSystem.Enemies[BattleSystem.EnemyPlayingID], BattleSystem.Turn, totalDamage);
        }
    }
}