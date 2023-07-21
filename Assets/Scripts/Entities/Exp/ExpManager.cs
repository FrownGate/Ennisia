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

    public Dictionary<int, int> PlayerlevelExperienceMap;
    public Dictionary<int, int> AccountlevelExperienceMap;
    public static event Action<int,LevelUpQuestEvent.LvlType> OnAccountLevelUp;
    public static event Action<int,LevelUpQuestEvent.LvlType> OnPlayerLevelUp;


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
        Debug.Log("player gain " + expToAdd);

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
            OnPlayerLevelUp?.Invoke(accountLevel,LevelUpQuestEvent.LvlType.Account);
        }

        PlayFabManager.Instance.Account.Exp = experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met à jour l'interface utilisateur
    }

    public void GainExperiencePlayer(int expToAdd)
    {
        var level = PlayFabManager.Instance.Player.Level;
        var experience = PlayFabManager.Instance.Player.Exp;

        if (level == PlayerlevelExperienceMap.Count)
            // Le joueur a atteint le niveau maximum, ne gagne plus d'expérience
            return;

        experience += expToAdd; // Ajoute l'expérience spécifiée
        Debug.Log("player gain " + expToAdd);

        while (PlayerlevelExperienceMap.ContainsKey(level + 1) && experience >= PlayerlevelExperienceMap[level + 1])
        {
            level++; // Incrémente le niveau
            experience -=
                PlayerlevelExperienceMap[level]; // Déduit l'expérience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Player.Level = level;
            PlayFabManager.Instance.Player.Exp = experience;
            OnPlayerLevelUp?.Invoke(level,LevelUpQuestEvent.LvlType.Player);
        }

        PlayFabManager.Instance.Player.Exp = experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met à jour l'interface utilisateur
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

                PlayerlevelExperienceMap[level] = experienceRequired; // Associe le niveau à l'expérience requise
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
        RewardsManager.GainXp += GainExperiencePlayer;
    }
    private void OnDisable()
    {
        RewardsManager.GainXp -= GainExperienceAccount;
        RewardsManager.GainXp -= GainExperiencePlayer;
    }
}