using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHitSound : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioClip hitSoundSFX;
    public AudioClip hitSoundRocketSFX;
    public AudioClip KnifeHitSoundSFX;
    public AudioClip hitBombSoundSFX;
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
