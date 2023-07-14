using UnityEngine;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;

public class ExpManager : MonoBehaviour
{
    //public Image ProgressBar; // R�f�rence � l'objet Image de la barre de progression
    //public Text ExpText; // R�f�rence au texte de l'exp�rience
    //public Text LevelText; // R�f�rence au texte du niveau

    public static ExpManager Instance { get; private set; }

    [Expandable] public XPRewardData AccounRewards;

    public Dictionary<int, int> PlayerlevelExperienceMap;
    public Dictionary<int, int> AccountlevelExperienceMap;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadLevelExperienceMap();
    }

    public void GainExperienceAccount(int expToAdd)
    {
        var accountLevel = PlayFabManager.Instance.Account.Level;
        var experience = PlayFabManager.Instance.Account.Exp;

        if (accountLevel == AccountlevelExperienceMap.Count)
            // Le joueur a atteint le niveau maximum, ne gagne plus d'exp�rience
            return;

        experience += expToAdd; // Ajoute l'exp�rience sp�cifi�e
        Debug.Log("player gain " + expToAdd);

        while (AccountlevelExperienceMap.ContainsKey(accountLevel + 1) &&
               experience >= AccountlevelExperienceMap[accountLevel + 1])
        {
            accountLevel++; // Incr�mente le niveau
            experience -=
                AccountlevelExperienceMap
                    [accountLevel]; // D�duit l'exp�rience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Account.Level = accountLevel;
            PlayFabManager.Instance.Account.Exp = experience;
            AccounRewards.LvlupReward(accountLevel);
        }

        PlayFabManager.Instance.Account.Exp = experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met � jour l'interface utilisateur
    }

    public void GainExperiencePlayer(int expToAdd)
    {
        var level = PlayFabManager.Instance.Player.Level;
        var experience = PlayFabManager.Instance.Player.Exp;

        if (level == PlayerlevelExperienceMap.Count)
            // Le joueur a atteint le niveau maximum, ne gagne plus d'exp�rience
            return;

        experience += expToAdd; // Ajoute l'exp�rience sp�cifi�e
        Debug.Log("player gain " + expToAdd);

        while (PlayerlevelExperienceMap.ContainsKey(level + 1) && experience >= PlayerlevelExperienceMap[level + 1])
        {
            level++; // Incr�mente le niveau
            experience -=
                PlayerlevelExperienceMap[level]; // D�duit l'exp�rience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Player.Level = level;
            PlayFabManager.Instance.Player.Exp = experience;
        }

        PlayFabManager.Instance.Player.Exp = experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met � jour l'interface utilisateur
    }

    private void LoadLevelExperienceMap()
    {
        //TODO -> Use CSVUtils
        PlayerlevelExperienceMap = new Dictionary<int, int>();

        var filePath = Path.Combine(Application.dataPath, "Resources/CSV/PlayerXpCSVExport.csv");

        var csvLines = File.ReadAllLines(filePath);

        foreach (var line in csvLines)
        {
            var values = line.Split(',');

            if (values.Length >= 2)
            {
                var level = int.Parse(values[0]);
                var experienceRequired = int.Parse(values[1]);

                PlayerlevelExperienceMap[level] = experienceRequired; // Associe le niveau � l'exp�rience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }

        //Account ###############################################################################################
        AccountlevelExperienceMap = new Dictionary<int, int>();

        filePath = Path.Combine(Application.dataPath, "Resources/CSV/AccountXpCSVExport.csv");

        csvLines = File.ReadAllLines(filePath);

        foreach (var line in csvLines)
        {
            var values = line.Split(',');

            if (values.Length >= 2)
            {
                var level = int.Parse(values[0]);
                var experienceRequired = int.Parse(values[1]);

                AccountlevelExperienceMap[level] = experienceRequired; // Associe le niveau � l'exp�rience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }
    }
}