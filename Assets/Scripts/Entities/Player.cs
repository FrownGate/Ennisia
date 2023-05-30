public class Player : Entity
{
    public Player()
    {
        MaxHp = 300;
        Damage = 15;
        Speed = 30000;
        CurrentHp = MaxHp / 2;
    }
}