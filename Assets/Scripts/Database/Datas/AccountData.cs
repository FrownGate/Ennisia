using System;

[Serializable]
public class AccountData
{
    public string Name;
    public int Level;
    public int Exp;
    public int Gender;
    public bool Tutorial;

    public AccountData(string username)
    {
        Name = username;
        Level = 1;
        Exp = 0;
        Gender = 0;
        Tutorial = false;
    }
}
