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
        yield return new WaitForSeconds(0.5f); //TODO -> Player attack movement animation Remove when have animation sprite sheet 
        
        if (BattleSystem.Targets == null || BattleSystem.Targets.Count == 0)
        {
            // Debug.LogWarning(BattleSystem.Targetables.)
            Debug.LogError("Targetables is null or empty");
            yield break;
        }
        BattleSystem.Player.AtkBar = 0;

        if (BattleSystem.Targets.Count == 0)
        {
            BattleSystem.DialogueText.text = "No targets selected";
            yield break;
        }
        
        BattleSystem.SkillOnTurn(_selectedSkill);
        BattleSystem.ReduceCooldown();

        yield return new WaitForSeconds(0.5f); //TODO -> remove this wait on battle simulation

        BattleSystem.RemoveDeadEnemies();
        BattleSystem.Targets.Clear();

        if (BattleSystem.Enemies.Count == 0) //TODO -> use BattleSystem.AllEnemiesDead()
        {
            BattleSystem.SetState(new Won(BattleSystem));
        }
        else
        {
            BattleSystem.SetState(new CheckTurn(BattleSystem));
        }
    }
}