using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;

public class ExperienceSystem : MonoBehaviour
{
    //public Image ProgressBar; // Référence à l'objet Image de la barre de progression
    //public Text ExpText; // Référence au texte de l'expérience
    //public Text LevelText; // Référence au texte du niveau



    
    private Dictionary<int, int> _PlayerlevelExperienceMap;
    private Dictionary<int, int> _AccountlevelExperienceMap;

    [Expandable] public XPRewardData Rewards;


    public static ExperienceSystem Instance { get; private set; }


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
        int _level = PlayFabManager.Instance.Account.Level;
        int _experience = PlayFabManager.Instance.Account.Exp;

        if (_level == _AccountlevelExperienceMap.Count)
        {
            // Le joueur a atteint le niveau maximum, ne gagne plus d'expérience
            return;
        }

        _experience += expToAdd; // Ajoute l'expérience spécifiée

        while (_AccountlevelExperienceMap.ContainsKey(_level + 1) && _experience >= _AccountlevelExperienceMap[_level + 1])
        {
            _level++; // Incrémente le niveau
            _experience -= _AccountlevelExperienceMap[_level]; // Déduit l'expérience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Account.Level = _level;
        }

        PlayFabManager.Instance.Account.Exp = _experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met à jour l'interface utilisateur
    }
    public void GainExperiencePlayer(int expToAdd)
    {
        int _level = PlayFabManager.Instance.Player.Level;
        int _experience = PlayFabManager.Instance.Player.Exp;

        if (_level == _PlayerlevelExperienceMap.Count)
        {
            // Le joueur a atteint le niveau maximum, ne gagne plus d'expérience
            return;
        }

        _experience += expToAdd; // Ajoute l'expérience spécifiée

        while (_PlayerlevelExperienceMap.ContainsKey(_level + 1) && _experience >= _PlayerlevelExperienceMap[_level + 1])
        {
            _level++; // Incrémente le niveau
            _experience -= _PlayerlevelExperienceMap[_level]; // Déduit l'expérience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Player.Level = _level;
            Rewards.LVLUPReward(_level);
        }

        PlayFabManager.Instance.Player.Exp = _experience;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met à jour l'interface utilisateur
    }

    private void LoadLevelExperienceMap()
    {
        //TODO -> Use CSVUtils
        _PlayerlevelExperienceMap = new();

        string filePath = Path.Combine(Application.dataPath, "Resources/CSV/PlayerXpCSVExport.csv");

        string[] csvLines = File.ReadAllLines(filePath);

        foreach (string line in csvLines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2)
            {
                int level = int.Parse(values[0]);
                int experienceRequired = int.Parse(values[1]);

                _PlayerlevelExperienceMap[level] = experienceRequired; // Associe le niveau à l'expérience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }
        //Account ###############################################################################################
        _AccountlevelExperienceMap = new();

        filePath = Path.Combine(Application.dataPath, "Resources/CSV/AccountXpCSVExport.csv");

        csvLines = File.ReadAllLines(filePath);

        foreach (string line in csvLines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2)
            {
                int level = int.Parse(values[0]);
                int experienceRequired = int.Parse(values[1]);

                _AccountlevelExperienceMap[level] = experienceRequired; // Associe le niveau à l'expérience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }

        // UpdateUI(); // Met à jour l'interface utilisateur
    }

    
    //private void UpdateUI()
    //{
    //    if (_levelExperienceMap.ContainsKey(_level + 1))
    //    {
    //        int experienceRequired = _levelExperienceMap[_level + 1];
    //        ExpText.text = _experience + " / " + experienceRequired; // Affiche l'expérience actuelle et l'expérience requise
    //        LevelText.text = _level.ToString(); // Affiche le niveau sans "Lvl :"

    //        float fillAmount = (float)_experience / experienceRequired; // Calcule le remplissage de la barre de progression
    //        ProgressBar.fillAmount = fillAmount; // Applique le remplissage à la barre de progression
    //    }
    //    else
    //    {
    //        ExpText.text = "Max"; // Affiche "Max" lorsque le joueur atteint le niveau maximum
    //        LevelText.text = _level.ToString(); // Affiche le niveau sans "Lvl :"
    //        ProgressBar.fillAmount = 1f; // Remplit complètement la barre de progression
    //    }
    //}
}