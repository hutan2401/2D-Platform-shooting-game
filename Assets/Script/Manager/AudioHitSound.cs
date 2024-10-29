using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHitSound : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioClip hitSoundSFX;
    public AudioClip hitSoundRocketSFX;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
