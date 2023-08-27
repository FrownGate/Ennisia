using NaughtyAttributes;
using System;
using UnityEngine;

public class PLaySFX : MonoBehaviour
{
    [SerializeField] private AudioClip audioSFX;

    public void PlaySFX()
    {
        if (audioSFX != null) AudioManager.Instance.Play(audioSFX.name);
    }

    protected virtual void OnMouseUpAsButton()
    {
        PlaySFX();
    }
}