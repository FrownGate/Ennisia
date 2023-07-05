using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    List<Sound> sounds = new List<Sound>();
    AudioClip[] audioClip;
    private float _saveVolume = 0.1f;
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
        audioClip = Resources.LoadAll<AudioClip>("SFX/");
        //create a Sound for eache clip
        foreach (AudioClip clip in audioClip)
        {
            Sound sound = new Sound();
            sound.clip = clip;
            //if BGM then set loop to true. To do : need to find a better way to check if it's a BGM
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
            sound.Source.volume = 0.2f; //default value, will be change by the audio setting in game.
            sound.Source.pitch = 1;
            sound.Source.loop = sound.loop;
        }
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("BGM1");
    }

    private void Update()
    {
        //To mute/Un mute
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("wui");
            foreach (var sound in sounds)
            {
                if (sound.Source.volume > 0)
                {
                    Debug.Log("mute");
                    _saveVolume = sound.Source.volume;
                    sound.Source.volume = 0;
                }
                else
                {
                    Debug.Log("unmute");
                    sound.Source.volume = _saveVolume;
                }
            }
        }
        // Volume up
        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            Debug.Log("up volume");
            foreach (var sound in sounds)
            {
                if(sound.Source.volume < 1)
                {
                    sound.Source.volume += _stepVolume;
                }
            }
        }
        // Volume down
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("down volume");
            foreach (var sound in sounds)
            {
                if (sound.Source.volume > 0)
                {
                    sound.Source.volume -= _stepVolume;
                }
            }
        }
    }

    private void OnClickSFX(int index)
    {
        FindObjectOfType<AudioManager>().Play("button" + index);
        Debug.Log("button sfx " + index + " played");
    }
    private void MissionStartBGM(MissionSO missionSO)
    {
        Debug.Log("mission Type:" + missionSO.Type);
        switch (missionSO.Type)
        {
            case MissionManager.MissionType.Raid:
                FindObjectOfType<AudioManager>().Play("raidsBGM");
                Debug.Log("Raid BGM played" + MissionManager.MissionType.Raid);
                break;

            case MissionManager.MissionType.Dungeon:
                FindObjectOfType<AudioManager>().Play("dungeonBGM");
                Debug.Log("Dungeon BGM played");
                break;

            case MissionManager.MissionType.MainStory:
                FindObjectOfType<AudioManager>().Play("mainStoryBGM");
                Debug.Log("MainStory BGM played");
                break;

            case MissionManager.MissionType.SideStory:
                FindObjectOfType<AudioManager>().Play("sideStoryBGM");
                Debug.Log("SideStory BGM played");
                break;

            case MissionManager.MissionType.AlternativeStory:
                FindObjectOfType<AudioManager>().Play("alternativeStoryBGM");
                Debug.Log("AlternativeStory BGM played");
                break;

            case MissionManager.MissionType.EndlessTower:
                FindObjectOfType<AudioManager>().Play("endlessTowerBGM");
                Debug.Log("EndlessTower BGM played");
                break;

            case MissionManager.MissionType.Expedition:
                FindObjectOfType<AudioManager>().Play("expeditionBGM");
                Debug.Log("Expedition BGM played");
                break;
        }
    }

    private void MissionDone(MissionSO missionSO)
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
        MissionManager.OnMissionStart += MissionStartBGM;

    }
    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= KillSFX;
        MissionManager.OnMissionComplete -= MissionDone;
        //Move event to SkillHUD
        //BattleSystem.OnClickSFX -= OnClickSFX;
        SceneButton.ChangeSceneSFX -= OnClickSFX;
        MissionManager.OnMissionStart -= MissionStartBGM;
    }
}
