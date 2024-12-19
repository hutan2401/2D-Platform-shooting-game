using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipSound
{
    public string nameSound;
    public AudioClip sound;
}


public class AudioHitSound : MonoBehaviour
{
    public static AudioHitSound Instance { get; private set; }

    public AudioSource SFXSource;
    public List<AudioClipSound> audioClipList;
    private Dictionary<string, AudioClip> hitSounds;
    //public AudioClip hitSoundSFX;
    //public AudioClip hitSoundRocketSFX;
    //public AudioClip KnifeHitSoundSFX;
    //public AudioClip hitBombSoundSFX;

    //public void PlaySFX(AudioClip clip)
    //{
    //    SFXSource.PlayOneShot(clip);
    //}
    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo không bị phá hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }

        // Khởi tạo Dictionary từ danh sách
        hitSounds = new Dictionary<string, AudioClip>();
        foreach (var clipSound in audioClipList)
        {
            if (!hitSounds.ContainsKey(clipSound.nameSound))
            {
                hitSounds.Add(clipSound.nameSound, clipSound.sound);
            }
            else
            {
                Debug.LogWarning("Duplicate sound name found: " + clipSound.nameSound);
            }
        }
    }

    public void PlaySFX(string clipName)
    {
        // Phát âm thanh theo tên
        if (hitSounds.TryGetValue(clipName, out AudioClip clip))
        {
            SFXSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }

    public void AddAudioClip(string name, AudioClip clip)
    {
        // Thêm âm thanh mới
        if (!hitSounds.ContainsKey(name))
        {
            hitSounds.Add(name, clip);
        }
        else
        {
            Debug.LogWarning("Audio clip already exists: " + name);
        }
    }

    public void RemoveAudioClip(string name)
    {
        // Xóa âm thanh theo tên
        if (hitSounds.ContainsKey(name))
        {
            hitSounds.Remove(name);
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + name);
        }
    }
}
