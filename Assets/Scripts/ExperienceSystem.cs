using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    public Image ProgressBar; // Référence à l'objet Image de la barre de progression
    public Text ExpText; // Référence au texte de l'expérience
    public Text LevelText; // Référence au texte du niveau

    [SerializeField] private int experienceToAdd = 5; // Montant d'expérience à ajouter lorsque le bouton est cliqué

    private int _level = 1;
    private int _experience = 0;
    private Dictionary<int, int> _levelExperienceMap;

    private void Awake()
    {
        LoadLevelExperienceMap();
    }

    public void GainExperience()
    {
        if (_level == _levelExperienceMap.Count)
        {
            // Le joueur a atteint le niveau maximum, ne gagne plus d'expérience
            return;
        }

        _experience += experienceToAdd; // Ajoute l'expérience spécifiée

        while (_levelExperienceMap.ContainsKey(_level + 1) && _experience >= _levelExperienceMap[_level + 1])
        {
            _level++; // Incrémente le niveau
            _experience -= _levelExperienceMap[_level]; // Déduit l'expérience requise pour atteindre le niveau suivant
        }

        UpdateUI(); // Met à jour l'interface utilisateur
    }

    private void LoadLevelExperienceMap()
    {
        _levelExperienceMap = new Dictionary<int, int>();

        string filePath = Path.Combine(Application.dataPath, "CSV/PlayerXpCSVExport.csv");

        string[] csvLines = File.ReadAllLines(filePath);

        foreach (string line in csvLines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2)
            {
                int level = int.Parse(values[0]);
                int experienceRequired = int.Parse(values[1]);

                _levelExperienceMap[level] = experienceRequired; // Associe le niveau à l'expérience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }

        UpdateUI(); // Met à jour l'interface utilisateur
    }

    private void UpdateUI()
    {
        if (_levelExperienceMap.ContainsKey(_level + 1))
        {
            int experienceRequired = _levelExperienceMap[_level + 1];
            ExpText.text = _experience + " / " + experienceRequired; // Affiche l'expérience actuelle et l'expérience requise
            LevelText.text = "Lvl: " + _level; // Affiche le niveau

            float fillAmount = (float)_experience / experienceRequired; // Calcule le remplissage de la barre de progression
            ProgressBar.fillAmount = fillAmount; // Applique le remplissage à la barre de progression
        }
        else
        {
            ExpText.text = "Max"; // Affiche "Max" lorsque le joueur atteint le niveau maximum
            LevelText.text = "Lvl: " + _level; // Affiche le niveau
            ProgressBar.fillAmount = 1f; // Remplit complètement la barre de progression
        }
    }
}
