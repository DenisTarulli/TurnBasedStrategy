using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

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

    [SerializeField] private AudioSource sfxSource;

    [System.Serializable]
    public class SoundData
    {
        public SoundType type;
        public AudioClip clip;
    }

    [SerializeField] private List<SoundData> sounds = new List<SoundData>();

    private Dictionary<SoundType, AudioClip> soundMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        soundMap = new Dictionary<SoundType, AudioClip>();

        foreach (SoundData sound in sounds)
        {
            if (!soundMap.ContainsKey(sound.type))
            {
                soundMap.Add(sound.type, sound.clip);
            }
        }
    }

    public void PlaySFX(SoundType type, float volume = 1f)
    {
        if (!soundMap.ContainsKey(type))
        {
            Debug.LogWarning("Sound not found: " + type);
            return;
        }

        sfxSource.PlayOneShot(soundMap[type], volume);
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
}