using System.Collections;
using UnityEngine;

public class SelectTarget : State
{
    private Skill _selectedSkill;
    //TODO -> clean commented code if unused

    public SelectTarget(BattleSystem battleSystem, Skill selectedSkill) : base(battleSystem)
    {
        _selectedSkill = selectedSkill;
    }

    public override IEnumerator Start()
    {
        BattleSystem.DialogueText.text = "You choose " + _selectedSkill;
        yield return new WaitForSeconds(1.0f);
    }

    public override IEnumerator Attack()
    {
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
        BattleSystem.Player.atkBar = 0;
        //Attack Button
        if (BattleSystem.Targetables.Count == 0)
        {
            BattleSystem.DialogueText.text = "No targets selected";
            yield break;
        }
        /*if (_selectedSkill.Cooldown > 0)
        {
            BattleSystem.DialogueText.text = "Skill in Cooldown";
            yield break;
        }*/

        BattleSystem.SkillOnTurn(_selectedSkill);
        BattleSystem.ReduceCooldown(); 
        
        Debug.Log("HP : " + BattleSystem.Enemies[0].CurrentHp);

        BattleSystem.Targetables.Clear();
        BattleSystem.Selected = false;

        yield return new WaitForSeconds(0.5f); //TODO -> remove this wait on battle simulation

        BattleSystem.RemoveDeadEnemies();

        if (BattleSystem.Enemies.Count == 0) //TODO -> use BattleSystem.AllEnemiesDead()
        {
            BattleSystem.SetState(new Won(BattleSystem));

            // BattleSystem.SetState(new WinState(BattleSystem));
            // yield break;
        }
        else
        {
            BattleSystem.SetState(new CheckTurn(BattleSystem));
        }
    }
}