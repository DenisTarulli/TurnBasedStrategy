using UnityEngine;

/// <summary>
/// Singleton generico encargado de REPRODUCIR sonidos (efectos posicionados
/// en el mundo 3D) y musica de fondo (loop, persistente entre escenas).
/// No sabe nada de personajes ni acciones - eso lo maneja UnitSound.cs.
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Efectos de sonido")]
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    [Header("Musica")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip defaultMusicClip;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Sobrevive a los cambios de escena (MainMenu -> Loading -> Partida)
        // para que la musica no se corte.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;

            if (defaultMusicClip != null)
            {
                musicSource.clip = defaultMusicClip;
                musicSource.Play();
            }
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volumeMultiplier = 1f)
    {
        if (clip == null)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(clip, position, sfxVolume * volumeMultiplier);
    }

    public void PlayRandomSound(AudioClip[] clips, Vector3 position, float volumeMultiplier = 1f)
    {
        if (clips == null || clips.Length == 0)
        {
            return;
        }

        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        PlaySound(randomClip, position, volumeMultiplier);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
}