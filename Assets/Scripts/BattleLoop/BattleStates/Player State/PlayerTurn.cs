using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        bool allAlliesDead = BattleSystem.Allies.All(ally => ally.IsDead);

        if (allAlliesDead)
        {
            BattleSystem.SetState(new Lost(BattleSystem));
            yield break;
        }
        
        BattleSystem.turn += 1;
        foreach (var skill in BattleSystem.SkillsButton)
        {
            skill.SetActive(true);
        }
        BattleSystem.dialogueText.text = "Your turn";
        yield return new WaitForSeconds(1.0f);
    }
}