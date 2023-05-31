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
        BattleSystem.dialogueText.text = "Select a target";
        yield return new WaitForSeconds(2.0f);
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
        foreach (var enemy in BattleSystem.Enemies)
        {
            Debug.LogWarning(enemy + " BattleSystem.Enemies");
        }
        foreach (var target in BattleSystem.Targetables)
        {
            Debug.LogWarning(target + " BattleSystem.Targetables");
        }

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


        _selectedSkill.Use(BattleSystem.Targetables, BattleSystem.Allies[0], BattleSystem.turn);


        /*foreach (var skill in BattleSystem.Allies[0].Skills)
        {
            skill.PassiveBeforeAttack(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn);
        }
        totalDamage += BattleSystem.Allies[0].Skills[_spellNumber].SkillBeforeUse(BattleSystem.Targetables, BattleSystem.Allies[0], BattleSystem.turn);
        totalDamage += BattleSystem.Allies[0].Skills[_spellNumber].Use(BattleSystem.Targetables, BattleSystem.Allies[0], BattleSystem.turn);
        totalDamage += BattleSystem.Allies[0].Skills[_spellNumber].AdditionalDamage(BattleSystem.Targetables, BattleSystem.Allies[0], BattleSystem.turn, totalDamage);
        BattleSystem.Allies[0].Skills[_spellNumber].SkillAfterDamage(BattleSystem.Targetables, BattleSystem.Allies[0], BattleSystem.turn, totalDamage);
        //skill after Attack

        foreach (var skill in BattleSystem.Allies[0].Skills)        
        {
            skill.PassiveAfterAttack(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn, totalDamage);
        }*/
        Debug.Log("HP : " + BattleSystem.Enemies[0].CurrentHp);
        Debug.LogWarning("Before clear : " + BattleSystem.Targetables.Count);
        BattleSystem.Targetables.Clear();
        Debug.LogWarning("After clear : " + BattleSystem.Targetables.Count);
        BattleSystem._selected = false;

        yield return new WaitForSeconds(0.5f);
        
        BattleSystem.RemoveDeadEnemies();
        //TODO: Check if all enemies are dead

        
        BattleSystem.SetState(new EnemyTurn(BattleSystem));
    }
}