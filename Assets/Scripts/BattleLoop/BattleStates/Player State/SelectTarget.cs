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