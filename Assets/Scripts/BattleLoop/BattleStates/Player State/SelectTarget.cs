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
        BattleSystem._selected = false;
        // Check if Player is not null
        /*if (BattleSystem.Player == null)
        {
            Debug.LogError("Player is null");
            yield break;
        }*/

        // Check if Targetables is not null and contains items
        if (BattleSystem.Targetables == null || BattleSystem.Targetables.Count == 0)
        {
            Debug.LogError("Targetables is null or empty");
            yield break;
        }

        //Attack Button
        foreach (var enemy in BattleSystem.Targetables)
        {
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
        }
        BattleSystem.RemoveDeadEnemies();
        Debug.LogWarning("Before clear : " + BattleSystem.Targetables.Count);
        BattleSystem.Targetables.Clear();
        Debug.LogWarning("After clear : " + BattleSystem.Targetables.Count);
        yield return new WaitForSeconds(0.5f);
        BattleSystem.SetState(new EnemyTurn(BattleSystem));
    }
}