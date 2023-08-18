using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();
    AudioClip[] audioClip;
    public float DefautlVolume = 0.01f;
    private float _saveVolume = 0.1f;
    public float BGMSaveVolume = 0.1f;
    public float SFXSaveVolume = 0.1f;
    private float _stepVolume = 0.01f;
    public static AudioManager Instance { get; private set; }

    private AudioMixer audioMixer;

    private AudioMixerGroup[] audioMixGroup;
    private Sound BGMCurentlyPlaying;
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
            Sound sound = new Sound
            {
                clip = clip
            };
            //if BGM then set loop to true.
            if (clip.name.Contains("BGM"))
            {
                sound.loop = true;
            }

            //add the create Sound to the list
            sounds.Add(sound);
            audioMixer = Resources.Load<AudioMixer>("Audio/AudioSettings");

            audioMixGroup = audioMixer.FindMatchingGroups("Master");

            BGMCurentlyPlaying = new Sound();
        }

        //for each sound in the list, create an AudioSource component and set the settings
        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();

            sound.name = sound.clip.name;
            if (sound.name.Contains("BGM"))
            {
                sound.Source.outputAudioMixerGroup = audioMixGroup[1];
            }

            if (sound.name.Contains("SFX"))
            {
                sound.Source.outputAudioMixerGroup = audioMixGroup[2];
            }
            sound.Source.clip = sound.clip;
            sound.Source.volume = DefautlVolume;
            sound.Source.pitch = 1;
            sound.Source.loop = sound.loop;
            if (sound.name.Contains("BGM")) sound.Source.volume = BGMSaveVolume;
            if (sound.name.Contains("SFX")) sound.Source.volume = SFXSaveVolume;
        }
    }

    private void Start()
    {
        Play("BGM MainMenu");
    }

    private void OnClickSFX(string sceneName)
    {
        if (!Play("SFX " + sceneName)) Play("SFXbutton1");
    }

    private void OnClickBGM(string sceneName)
    {
        Play("BGM " + sceneName);
    }

    private void MissionStartBGM(MissionSO missionSO)
    {
        Debug.Log("mission Type:" + missionSO.Type);
        switch (missionSO.Type)
        {
            case MissionType.Raid:
                Play("SFX RaidStart");
                Play("RaidBgm");
                Debug.Log("Raid BGM played");
                break;

            case MissionType.Dungeon:
                Play("SFX RaidStart");
                Play("RaidBgm");
                Debug.Log("Dungeon BGM played");
                break;

            case MissionType.MainStory:
                Play("StoryBgm");
                Play("SFX StoryStart");
                Debug.Log("MainStory BGM played");
                break;

            case MissionType.SideStory:
                Play("StoryBgm");
                Play("SFX StoryStart");
                Debug.Log("SideStory BGM played");
                break;

            case MissionType.AlternativeStory:
                Play("alternativeStoryBGM");
                Debug.Log("AlternativeStory BGM played");
                break;

            case MissionType.EndlessTower:
                Play("endlessTowerBGM");
                Debug.Log("EndlessTower BGM played");
                break;

            case MissionType.Expedition:
                Play("StoryBgm");
                Play("SFX StoryStart");
                Debug.Log("Expedition BGM played");
                break;
        }
    }

    private void MissionDone(MissionSO mission)
    {
        Play("SFX MissionDone");
    }

    private void KillSFX(string name)
    {
        Play(name + "die");
        Debug.Log(name + " die SFX played");
    }

    public bool Play(string name)
    {
        Sound s = sounds.Find(x => x.name.Contains(name));
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return false;
        }

        if (name.Contains("BGM"))
        {
            if (name != BGMCurentlyPlaying.name)
            {
                foreach (var sound in sounds.Where(sound => sound.name.Contains("BGM")))
                {
                    sound.Source.Stop();
                }
                BGMCurentlyPlaying = s;
            }else return true;
        }
        
        s.Source.Play();
        Debug.Log(name + " is playing");
        return true;
    }

    private void OnEnable()
    {
        BattleSystem.OnEnemyKilled += KillSFX;
        MissionManager.OnMissionComplete += MissionDone;
        //Move event to SkillHUD
        //BattleSystem.OnClickSFX += OnClickSFX;
        SceneButton.PlaySFXOnSceneChange += OnClickSFX;
        SceneButton.PlayMusicOnSceneChange += OnClickBGM;
        ShowStoryAct.Onclick += OnClickSFX;
        MissionManager.OnMissionStart += MissionStartBGM;
    }

    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= KillSFX;
        MissionManager.OnMissionComplete -= MissionDone;
        //Move event to SkillHUD
        //BattleSystem.OnClickSFX -= OnClickSFX;
        SceneButton.PlaySFXOnSceneChange -= OnClickSFX;
        SceneButton.PlayMusicOnSceneChange -= OnClickBGM;
        ShowStoryAct.Onclick -= OnClickSFX;
        MissionManager.OnMissionStart -= MissionStartBGM;
    }
}