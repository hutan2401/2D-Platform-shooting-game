using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipEntry
{
    public string name;       // Tên âm thanh
    public AudioClip clip;    // Đoạn âm thanh
}

//[System.Serializable]
//public class BulletSoundEntry
//{
//    public string bulletType; // Tên loại đạn
//    public AudioClip clip;    // Đoạn âm thanh bắn đạn
//}
public class ManagerAudioSound : MonoBehaviour
{
    public static ManagerAudioSound Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource; // Nguồn âm thanh SFX
    public AudioSource musicSource; // Nguồn nhạc nền (nếu cần)

    [Header("Hit Sounds")]
    public List<AudioClipEntry> hitSounds; // Danh sách âm thanh va chạm
    private Dictionary<string, AudioClip> hitSoundDict;

    [Header("Explode Sounds")]
    public List<AudioClipEntry> explodeSounds; 
    private Dictionary<string, AudioClip> explodeSoundDict;

    [Header("Bullet Sounds")]
    public List<AudioClipEntry> bulletSounds; // Danh sách âm thanh đạn
    private Dictionary<string, AudioClip> bulletSoundDict;

    private float soundCooldown = 0.25f;
    private float lastSoundPlayTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo không bị phá hủy khi chuyển Scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Khởi tạo Dictionaries
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        hitSoundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in hitSounds)
        {
            if (!hitSoundDict.ContainsKey(sound.name))
                hitSoundDict.Add(sound.name, sound.clip);
        }

        explodeSoundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in explodeSounds)
        {
            if (!explodeSoundDict.ContainsKey(sound.name))
                explodeSoundDict.Add(sound.name, sound.clip);
        }

        bulletSoundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in bulletSounds)
        {
            if (!bulletSoundDict.ContainsKey(sound.name))
                bulletSoundDict.Add(sound.name, sound.clip);
        }
    }

    #region Hit Sound Methods
    public void PlayHitSound(string name)
    {
        if (hitSoundDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Hit sound '{name}' not found!");
        }
    }
    #endregion

    #region Bullet Sound Methods
    public void PlayBulletSound(string bulletType)
    {
        if (Time.time - lastSoundPlayTime < soundCooldown) return;

        if (bulletSoundDict.TryGetValue(bulletType, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
            lastSoundPlayTime = Time.time;
        }
        else
        {
            Debug.LogWarning($"Bullet sound for '{bulletType}' not found!");
        }
    }
    #endregion

    #region Sound Explore
    public void PlayExplodeSound(string clipName)
    {
        if (explodeSoundDict.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Explode sound '{clipName}' not found!");
        }
    }
    #endregion

    #region Music Methods
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicSource.isPlaying) musicSource.Stop();

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying) musicSource.Stop();
    }
    #endregion
}
