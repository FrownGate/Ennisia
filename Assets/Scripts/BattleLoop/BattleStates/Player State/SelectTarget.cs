using System.Collections;
using UnityEngine;

public class SelectTarget : State
{
    private Skill _selectedSkill;

    public SelectTarget(BattleSystem battleSystem, Skill selectedSkill) : base(battleSystem)
    {
        _selectedSkill = selectedSkill;
    }

    public override IEnumerator Start()
    {
        BattleSystem.dialogueText.text = "You choose " + _selectedSkill;
        yield return new WaitForSeconds(1.0f);
    }

    public override IEnumerator Attack()
    {
        float totalDamage = 0;
        // Check if Player is not null
        /*if (BattleSystem.Player == null)
        {
            Debug.LogError("Player is null");
            yield break;
        }*/

        // Check if Targetables is not null and contains items
        if (BattleSystem.Targetables == null || BattleSystem.Targetables.Count == 0)
        {
            // Debug.LogWarning(BattleSystem.Targetables.)
            Debug.LogError("Targetables is null or empty");
            yield break;
        }

        //Attack Button
        if (BattleSystem.Targetables.Count == 0)
        {
            BattleSystem.dialogueText.text = "No targets selected";
            yield break;
        }
        if(_selectedSkill.Cooldown > 0)
        {
            BattleSystem.dialogueText.text = "Skill in Cooldown";
            yield break;
        }


        _selectedSkill.Use(BattleSystem.Targetables, BattleSystem.Player, BattleSystem.turn);


        foreach (var skill in BattleSystem.Player.Skills)
        {
            skill.PassiveBeforeAttack(BattleSystem.Enemies, BattleSystem.Player, BattleSystem.turn);
        }
        totalDamage += _selectedSkill.SkillBeforeUse(BattleSystem.Targetables, BattleSystem.Player, BattleSystem.turn);
        totalDamage += _selectedSkill.Use(BattleSystem.Targetables, BattleSystem.Player, BattleSystem.turn);
        totalDamage += _selectedSkill.AdditionalDamage(BattleSystem.Targetables, BattleSystem.Player, BattleSystem.turn, totalDamage);
        _selectedSkill.SkillAfterDamage(BattleSystem.Targetables, BattleSystem.Player, BattleSystem.turn, totalDamage);

        foreach (var skill in BattleSystem.Player.Skills)        
        {
            skill.PassiveAfterAttack(BattleSystem.Enemies, BattleSystem.Player, BattleSystem.turn, totalDamage);
        }

        Debug.Log("HP : " + BattleSystem.Enemies[0].CurrentHp);
        BattleSystem.Targetables.Clear();
        BattleSystem._selected = false;

        yield return new WaitForSeconds(0.5f);

        BattleSystem.RemoveDeadEnemies();

        if (BattleSystem.Enemies.Count == 0)
        {
            BattleSystem.SetState(new Won(BattleSystem));

            // BattleSystem.SetState(new WinState(BattleSystem));
            // yield break;
        }
        else
        {
            BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }
    }
}