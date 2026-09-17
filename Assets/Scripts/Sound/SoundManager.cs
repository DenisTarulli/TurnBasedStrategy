using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Hay mas de un SoundManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }
}