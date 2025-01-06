using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AudioClipEntry
{
    public string name;       // Tên âm thanh
    public AudioClip clip;    // Đoạn âm thanh
}

[System.Serializable]
public class SceneMusicEntry
{
    public string sceneName;     // Tên Scene
    public AudioClip musicClip;  // Đoạn nhạc cho Scene
}
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

    [Header("Scene Music")]
    public List<SceneMusicEntry> sceneMusicList; // Danh sách nhạc từng Scene
    private Dictionary<string, AudioClip> sceneMusicDict; // Từ điển nhạc từng Scene

    [Header("Special Music")]
    public AudioClip bossMusic;        // Music during the boss fight
    public AudioClip victoryMusic;     // Music after defeating the boss

    private bool bossMusicPlayed = false;       // Prevent boss music replay
    private bool victoryMusicPlayed = false;

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

        sceneMusicDict = new Dictionary<string, AudioClip>();
        foreach (var entry in sceneMusicList)
        {
            if (!sceneMusicDict.ContainsKey(entry.sceneName))
                sceneMusicDict.Add(entry.sceneName, entry.musicClip);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Lấy tên Scene hiện tại
        string currentSceneName = scene.name;
        bossMusicPlayed = false;
        victoryMusicPlayed = false;
        // Kiểm tra và phát nhạc tương ứng với Scene
        if (sceneMusicDict.TryGetValue(currentSceneName, out AudioClip musicClip))
        {
            PlayMusic(musicClip);
        }
        else
        {
            StopMusic();    
            Debug.LogWarning($"No music assigned for Scene '{currentSceneName}'!");
        }
    }
    #endregion
    #region Boss and Victory Music
    public void PlayBossMusic()
    {
        if (!bossMusicPlayed)
        {
            bossMusicPlayed = true;
            PlayMusic(bossMusic);
        }
    }

    public void PlayVictoryMusic()
    {
        if (!victoryMusicPlayed)
        {
            victoryMusicPlayed = true;
            PlayMusic(victoryMusic);
        }
    }
    #endregion
}
