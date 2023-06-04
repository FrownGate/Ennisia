using System.Collections;

public class Won : State
{
    public Won(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        foreach (var skill in BattleSystem.Allies[0].Skills)
        {
            skill.TakeOffStats(BattleSystem.Enemies, BattleSystem.Allies[0], 0); // constant passive at battle end
        }

        BattleSystem.SetSkillButtonsActive(false);

        BattleSystem.DialogueText.text = "YOU WON THE FIGHT";
        BattleSystem.WonPopUp.SetActive(true);
        yield break;
    }
}