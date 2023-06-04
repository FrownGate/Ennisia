using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    public Image ProgressBar; // R�f�rence � l'objet Image de la barre de progression
    public Text ExpText; // R�f�rence au texte de l'exp�rience
    public Text LevelText; // R�f�rence au texte du niveau

    [SerializeField] private int _expToAdd = 5; // Montant d'exp�rience � ajouter lorsque le bouton est cliqu�

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
            // Le joueur a atteint le niveau maximum, ne gagne plus d'exp�rience
            return;
        }

        _experience += _expToAdd; // Ajoute l'exp�rience sp�cifi�e

        while (_levelExperienceMap.ContainsKey(_level + 1) && _experience >= _levelExperienceMap[_level + 1])
        {
            _level++; // Incr�mente le niveau
            _experience -= _levelExperienceMap[_level]; // D�duit l'exp�rience requise pour atteindre le niveau suivant
        }

        UpdateUI(); // Met � jour l'interface utilisateur
    }

    private void LoadLevelExperienceMap()
    {
        //TODO -> Use CSVUtils
        _levelExperienceMap = new Dictionary<int, int>();

        string filePath = Path.Combine(Application.dataPath, "Editor/CSV/PlayerXpCSVExport.csv");

        string[] csvLines = File.ReadAllLines(filePath);

        foreach (string line in csvLines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2)
            {
                int level = int.Parse(values[0]);
                int experienceRequired = int.Parse(values[1]);

                _levelExperienceMap[level] = experienceRequired; // Associe le niveau � l'exp�rience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }

        UpdateUI(); // Met � jour l'interface utilisateur
    }

    private void UpdateUI()
    {
        if (_levelExperienceMap.ContainsKey(_level + 1))
        {
            int experienceRequired = _levelExperienceMap[_level + 1];
            ExpText.text = _experience + " / " + experienceRequired; // Affiche l'exp�rience actuelle et l'exp�rience requise
            LevelText.text = _level.ToString(); // Affiche le niveau sans "Lvl :"

            float fillAmount = (float)_experience / experienceRequired; // Calcule le remplissage de la barre de progression
            ProgressBar.fillAmount = fillAmount; // Applique le remplissage � la barre de progression
        }
        else
        {
            ExpText.text = "Max"; // Affiche "Max" lorsque le joueur atteint le niveau maximum
            LevelText.text = _level.ToString(); // Affiche le niveau sans "Lvl :"
            ProgressBar.fillAmount = 1f; // Remplit compl�tement la barre de progression
        }
    }
}