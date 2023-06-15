using UnityEngine;

public abstract class Pet
{
    public int AffinityLevel { get; private set; }
    public int ActualXP { get; private set; }
    public int ToGetXp { get; private set; }

    private PetSO _data;
    public string _name; //will be private
    private Sprite _icon;
    private bool _obtained;

    public Pet()
    {
        _data = Resources.Load<PetSO>("SO/Pets/" + GetType().Name);
        _name = _data.name;
        _icon = _data.Icon;
        Debug.Log($"Loaded {_name} datas !");

        //TODO -> set obtained, level and actual xp with database
        _obtained = false;
        AffinityLevel = 0;
        ToGetXp = 100;

        if (_obtained) Init();
    }

    public virtual void Init() { }

    public virtual void GetPet(Player player)
    {
        _obtained = true;
        Init();
    }

    public virtual void Play()
    {
        //TODO -> Only one Play per day accross all pets
        Debug.Log("Successfully played with" + _name);
        ActualXP += 50 + 20 * AffinityLevel;
        LevelUp();
    }

    public virtual void LevelUp()
    {
        if (ActualXP >= ToGetXp)
        {
            if (AffinityLevel < 10)
            {
                AffinityLevel++;
                ActualXP -= ToGetXp;
                ToGetXp *= 2;
                Debug.Log("Level up to level : " + AffinityLevel);
            }
            else
            {
                ActualXP = ToGetXp;
            }
        }

    }
}