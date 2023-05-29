using System.Collections;
using UnityEngine;

public class SelectTarget : SelectSpell
{
    public SelectTarget(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.dialogueText.text = "Select" + BattleSystem.SelectedTargetNumber + "  target";
        yield return new WaitForSeconds(2.0f);
    }

    public override IEnumerator Attack()
    {
        float totalDamage = 0 ;
        BattleSystem._selected = 0;
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
            // Check if enemy is not null
            if (enemy == null)
            {
                Debug.LogError("Enemy in Targetables is null");
                continue;
            }

            foreach (var skill in BattleSystem.Allies[0].Skills)
            {
                skill.PassiveBeforeAttack(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn);
            }
            totalDamage += BattleSystem.Allies[0].Skills[_spellNumber].SkillBeforeUse(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn);
            totalDamage += BattleSystem.Allies[0].Skills[_spellNumber].Use(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn);
            totalDamage += BattleSystem.Allies[0].Skills[_spellNumber].AdditionalDamage(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn, totalDamage);
            //skill after Attack

            foreach (var skill in BattleSystem.Allies[0].Skills)
            {
                skill.PassiveAfterAttack(BattleSystem.Enemies, BattleSystem.Allies[0], BattleSystem.turn, totalDamage);
            }
        }

        BattleSystem.Targetables.Clear();
        yield return new WaitForSeconds(0.5f);
        BattleSystem.SetState(new EnemyTurn(BattleSystem)); 
    }
}