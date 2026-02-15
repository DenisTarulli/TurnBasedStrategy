using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // =========================
    // ENUMS
    // =========================
    public enum SoundType
    {
        GolpeMago,
        AbrirCofre,
        ArmaDisparo,
        GranadaExplosion,
        MuertePersonaje,
        AbrirPuerta,
        RecolectarLlave,
        GoblinSarten
    }

    public enum MusicType
    {
        Gameplay,
        Shop
    }

    // =========================
    // AUDIO SOURCES
    // =========================
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    // =========================
    // SFX DATA
    // =========================
    [System.Serializable]
    public class SoundData
    {
        public SoundType type;
        public AudioClip clip;
        [Range(0f, 10f)] public float delay;
        [Range(0f, 2f)] public float volume = 1f;
    }

    [SerializeField] private List<SoundData> sounds = new List<SoundData>();
    private Dictionary<SoundType, SoundData> soundMap;

    // =========================
    // MUSIC DATA
    // =========================
    [System.Serializable]
    public class MusicData
    {
        public MusicType type;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<MusicData> musics = new List<MusicData>();
    private Dictionary<MusicType, MusicData> musicMap;

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build SFX map
        soundMap = new Dictionary<SoundType, SoundData>();
        foreach (SoundData sound in sounds)
        {
            if (!soundMap.ContainsKey(sound.type))
            {
                soundMap.Add(sound.type, sound);
            }
        }

        // Build Music map
        musicMap = new Dictionary<MusicType, MusicData>();
        foreach (MusicData music in musics)
        {
            if (!musicMap.ContainsKey(music.type))
            {
                musicMap.Add(music.type, music);
            }
        }
    }

    // =========================
    // SFX
    // =========================
    public void PlaySFX(SoundType type)
    {
        if (!soundMap.ContainsKey(type))
        {
            Debug.LogWarning("SFX not found: " + type);
            return;
        }

        SoundData data = soundMap[type];

        if (data.delay > 0f)
        {
            StartCoroutine(PlayWithDelay(data));
        }
        else
        {
            sfxSource.PlayOneShot(data.clip, data.volume);
        }
    }

    private IEnumerator PlayWithDelay(SoundData data)
    {
        yield return new WaitForSeconds(data.delay);
        sfxSource.PlayOneShot(data.clip, data.volume);
    }

    public void StopAllSFX()
    {
        sfxSource.Stop();
    }

    public void PauseSFX()
    {
        sfxSource.Pause();
    }

    public void ResumeSFX()
    {
        sfxSource.UnPause();
    }

    // =========================
    // MUSIC
    // =========================
    public void PlayMusic(MusicType type)
    {
        if (!musicMap.ContainsKey(type))
        {
            Debug.LogWarning("Music not found: " + type);
            return;
        }

        MusicData data = musicMap[type];

        if (musicSource.clip == data.clip && musicSource.isPlaying)
        {
            return; // ya está sonando
        }

        musicSource.clip = data.clip;
        musicSource.volume = data.volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void RestartMusic()
    {
        if (musicSource == null || musicSource.clip == null) return;

        musicSource.Stop();
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
}