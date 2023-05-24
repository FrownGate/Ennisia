using UnityEngine;

public class AccountData : Data
{
    public string Name;
    public int Level;
    public int Exp;
    public int Gender;
    public bool Tutorial;

    public AccountData(string username)
    {
        ClassName = "Account";
        Name = username;
        Level = 1;
        Exp = 0;
        Gender = 0;
        Tutorial = false;
    }

    public override void UpdateData(string json)
    {
        AccountData data = JsonUtility.FromJson<AccountData>(json);
        Name = data.Name;
        Level = data.Level;
        Exp = data.Exp;
        Gender = data.Gender;
        Tutorial = data.Tutorial;
    }
}
