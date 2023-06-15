public class Extermination : Quest
{
    int enemyKilled;
    public override void CheckCondition()
    {
        enemyKilled++;
        if(enemyKilled >= 100) 
        { 
            GiveRewards();
        }
    }

    private void OnEnable()
    {
        BattleSystem.OnEnemyKilled += CheckCondition;
    }
    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= CheckCondition;
    }
}