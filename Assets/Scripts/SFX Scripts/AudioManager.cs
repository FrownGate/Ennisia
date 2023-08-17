using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();
    AudioClip[] audioClip;
    public float DefautlVolume = 0.05f;
    private float _saveVolume = 0.1f;
    public float BGMSaveVolume = 0.1f;
    public float SFXSaveVolume = 0.1f;
    private float _stepVolume = 0.01f;
    public static AudioManager Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        //get all audio in resource folder : SFX/
        audioClip = Resources.LoadAll<AudioClip>("Audio/");
        //create a Sound for eache clip
        foreach (AudioClip clip in audioClip)
        {
            Sound sound = new Sound();
            sound.clip = clip;
            //if BGM then set loop to true.
            if (clip.name.Contains("BGM"))
            {
                sound.loop = true;
            }
            //add the create Sound to the list
            sounds.Add(sound);
        }
        //for each sound in the list, create an AudioSource component and set the settings
        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.name = sound.clip.name;
            sound.Source.clip = sound.clip;
            sound.Source.volume = DefautlVolume;
            sound.Source.pitch = 1;
            sound.Source.loop = sound.loop;
            if (sound.name.Contains("Bgm")) sound.Source.volume = BGMSaveVolume;
            if (sound.name.Contains("SFX")) sound.Source.volume = SFXSaveVolume;

        }
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("MenusBgm");
    }
    private void OnClickSFX(int index)
    {
        FindObjectOfType<AudioManager>().Play("SFXbutton" + index);
        //Debug.Log("button sfx " + index + " played");
    }
    private void MissionStartBGM(MissionSO missionSO)
    {
        Debug.Log("mission Type:" + missionSO.Type);
        switch (missionSO.Type)
        {
            case MissionType.Raid:
                FindObjectOfType<AudioManager>().Play("RaidBgm");
                Debug.Log("Raid BGM played" + MissionType.Raid);
                break;

            case MissionType.Dungeon:
                FindObjectOfType<AudioManager>().Play("RaidBgm");
                Debug.Log("Dungeon BGM played");
                break;

            case MissionType.MainStory:
                FindObjectOfType<AudioManager>().Play("StoryBgm");
                Debug.Log("MainStory BGM played");
                break;

            case MissionType.SideStory:
                FindObjectOfType<AudioManager>().Play("StoryBgm");
                Debug.Log("SideStory BGM played");
                break;

            case MissionType.AlternativeStory:
                FindObjectOfType<AudioManager>().Play("alternativeStoryBGM");
                Debug.Log("AlternativeStory BGM played");
                break;

            case MissionType.EndlessTower:
                FindObjectOfType<AudioManager>().Play("endlessTowerBGM");
                Debug.Log("EndlessTower BGM played");
                break;

            case MissionType.Expedition:
                FindObjectOfType<AudioManager>().Play("StoryBgm");
                Debug.Log("Expedition BGM played");
                break;
        }
    }

    private void MissionDone(MissionSO mission)
    {
        FindObjectOfType<AudioManager>().Play("missionDone");
    }

    private void KillSFX(string name)
    {
        FindObjectOfType<AudioManager>().Play(name + "die");
        Debug.Log(name + " die SFX played");
    }

    public void Play(string name)
    {
        Sound s = sounds.Find(x => x.name.Contains(name));
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        s.Source.Play();
    }

    private void OnEnable()
    {
        BattleSystem.OnEnemyKilled += KillSFX;
        MissionManager.OnMissionComplete += MissionDone;
        //Move event to SkillHUD
        //BattleSystem.OnClickSFX += OnClickSFX;
        SceneButton.ChangeSceneSFX += OnClickSFX;
        ShowStoryAct.Onclick += OnClickSFX;
        MissionManager.OnMissionStart += MissionStartBGM;

    }
    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= KillSFX;
        MissionManager.OnMissionComplete -= MissionDone;
        //Move event to SkillHUD
        //BattleSystem.OnClickSFX -= OnClickSFX;
        SceneButton.ChangeSceneSFX -= OnClickSFX;
        ShowStoryAct.Onclick -= OnClickSFX;
        MissionManager.OnMissionStart -= MissionStartBGM;
    }
}
