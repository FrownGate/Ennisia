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
        if (_spellNumber == 0)
        {
            foreach (var enemy in BattleSystem.Targetables)
            {
                // Check if enemy is not null
                if (enemy == null)
                {
                    Debug.LogError("Enemy in Targetables is null");
                    continue;
                }

                enemy.TakeDamage(BattleSystem.Allies[0].Attack);
            }
        }
        else if (_spellNumber == 1)//Spell Button 1 
        {

        }
        else if (_spellNumber == 2)//Spell Button 2 
        {

        }
        BattleSystem.Targetables.Clear();
        yield return new WaitForSeconds(0.5f);
        BattleSystem.SetState(new EnemyTurn(BattleSystem)); 
    }
}