using System.Collections;
using UnityEngine;

public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.turn += 1;
        foreach (var skill in BattleSystem.SkillsButton)
        {
            skill.SetActive(true);
        }
        BattleSystem.dialogueText.text = "Your turn";
        yield return new WaitForSeconds(1.0f);
    }
}