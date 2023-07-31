using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;

public class ExpManager : MonoBehaviour
{
    //public Image ProgressBar; // Référence à l'objet Image de la barre de progression
    //public Text ExpText; // Référence au texte de l'expérience
    //public Text LevelText; // Référence au texte du niveau

    public static ExpManager Instance { get; private set; }

    [Expandable] public XPRewardData AccounRewards;

    public Dictionary<int, int> AccountlevelExperienceMap;
    public static event Action<int,LevelUpQuestEvent.LvlType> OnAccountLevelUp;


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
            // Le joueur a atteint le niveau maximum, ne gagne plus d'expérience
            return;

        experience += expToAdd; // Ajoute l'expérience spécifiée
        Debug.Log("account gain " + expToAdd);

        while (AccountlevelExperienceMap.ContainsKey(accountLevel + 1) &&
               experience >= AccountlevelExperienceMap[accountLevel + 1])
        {
            accountLevel++; // Incrémente le niveau
            experience -=
                AccountlevelExperienceMap
                    [accountLevel]; // Déduit l'expérience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Account.Level = accountLevel;
            PlayFabManager.Instance.Account.Exp = experience;
            AccounRewards.LvlupReward(accountLevel);
            Debug.Log("account levelup ");
            OnAccountLevelUp?.Invoke(accountLevel,LevelUpQuestEvent.LvlType.Account);
        }

        PlayFabManager.Instance.Account.Exp = experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met à jour l'interface utilisateur
    }


    private void LoadLevelExperienceMap()
    {

        //Account ###############################################################################################
        AccountlevelExperienceMap = new Dictionary<int, int>();

        string filePath = Path.Combine(Application.dataPath, "Resources/CSV/AccountXpCSVExport.csv");

        string[] csvLines = File.ReadAllLines(filePath);

        foreach (var line in csvLines)
        {
            var values = line.Split(',');

            if (values.Length >= 2)
            {
                var level = int.Parse(values[0]);
                var experienceRequired = int.Parse(values[1]);

                AccountlevelExperienceMap[level] = experienceRequired; // Associe le niveau à l'expérience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }
    }


    private void OnEnable()
    {
        RewardsManager.GainXp += GainExperienceAccount;
    }
    private void OnDisable()
    {
        RewardsManager.GainXp -= GainExperienceAccount;
    }
}