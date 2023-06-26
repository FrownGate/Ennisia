using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

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
            foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.name = sound.clip.name;
            sound.Source.clip = sound.clip;
            sound.Source.volume = sound.volume;
            sound.Source.pitch = sound.pitch;
            sound.Source.loop = sound.loop;
        }
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("BGM1");
    }

    private void OnClickSFX(int index)
    {
        FindObjectOfType<AudioManager>().Play("button"+index);
        Debug.Log("button sfx "+index+" played");
    }

    private void MissionDone(MissionSO missionSO)
    {
        FindObjectOfType<AudioManager>().Play("missionDone");
        Debug.Log("Mission Done SFX played");
    }

    private void KillSFX(string name)
    {
        FindObjectOfType<AudioManager>().Play(name+"die");
        Debug.Log(name + " die SFX played");

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
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
    }
    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= KillSFX;
        MissionManager.OnMissionComplete -= MissionDone;
        BattleSystem.OnClickSFX -= OnClickSFX;
    }
}
