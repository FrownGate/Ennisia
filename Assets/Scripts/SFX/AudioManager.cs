using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    List<Sound> sounds = new List<Sound>();
    AudioClip[] audioClip;
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

        audioClip = Resources.LoadAll<AudioClip>("SFX/");
        foreach (AudioClip clip in audioClip)
        {
            Sound sound = new Sound();
            sound.clip = clip;
            if (clip.name.Contains("BGM"))
            {
                sound.loop = true;
            }
            sounds.Add(sound);
            Debug.Log("sounds count : "+sounds.Count);
        }

        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.name = sound.clip.name;
            Debug.Log("sound name : " + sound.name);
            sound.Source.clip = sound.clip;
            sound.Source.volume = 0.2f;
            sound.Source.pitch = 1;
            sound.Source.loop = sound.loop;
        }
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("BGM1");
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

        FindObjectOfType<AudioManager>().Play("BGM1 played");
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
        BattleSystem.OnClickSFX += OnClickSFX;
        SceneButton.ChangeSceneSFX += OnClickSFX;
        MissionManager.OnMissionStart += MissionStartBGM;

    }
    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= KillSFX;
        MissionManager.OnMissionComplete -= MissionDone;
        BattleSystem.OnClickSFX -= OnClickSFX;
        SceneButton.ChangeSceneSFX -= OnClickSFX;
        MissionManager.OnMissionStart -= MissionStartBGM;
    }
}
