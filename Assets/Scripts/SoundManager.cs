using UnityEngine;
using System.Collections;
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
        [Range(0f, 10f)] public float delay;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<SoundData> sounds = new List<SoundData>();

    private Dictionary<SoundType, SoundData> soundMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        soundMap = new Dictionary<SoundType, SoundData>();

        foreach (SoundData sound in sounds)
        {
            if (!soundMap.ContainsKey(sound.type))
            {
                soundMap.Add(sound.type, sound);
            }
        }
    }

    public void PlaySFX(SoundType type)
    {
        if (!soundMap.ContainsKey(type))
        {
            Debug.LogWarning("Sound not found: " + type);
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

    public void StopSFX()
    {
        sfxSource.Stop();
    }
}