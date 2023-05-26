public class Player : Entity
{
    public Player()
    {
        MaxHp = 300;
        Attack = 15;
        Speed = 30000;
        CurrentHp = MaxHp / 2;

        Skills.Add(new Bonk());
        Skills.Add(new TidalCrash());
        Skills.Add(new BLueDragonWraith());
    }
}