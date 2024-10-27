using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;


[System.Serializable]
public class BulletSound
{
    public string bulletTypeName;
    public AudioClip shootingSound;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;

    public List<BulletSound> bulletSounds;
    private Dictionary<string, BulletSound> bulletSoundDict;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bulletSoundDict = new Dictionary<string, BulletSound>();
        foreach (var bulletSound in bulletSounds)
        {
            bulletSoundDict[bulletSound.bulletTypeName] = bulletSound;
        }
    }

    public void PlayShootingSound(string bulletTypeName)
    {
        if (bulletSoundDict.TryGetValue(bulletTypeName, out var bulletSound))
        {
            if(bulletSound.shootingSound != null)
            {
                audioSource.PlayOneShot(bulletSound.shootingSound);
            }
        }
        else
        {
            Debug.LogWarning("Sound not found for bullet type: " + bulletTypeName);
        }
    }
}
